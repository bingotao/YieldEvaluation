using Aspose.Cells;
using JXGIS.Common.BaseLib;
using JXGIS.Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXGIS.YieldEvaluation.Business
{
    public class HomeUtils
    {
        public static SortResult _SortResult = null;
        public static List<QYGM0> _Companies = null;
        public static List<Error> _Errors = null;

        public static List<DM_GY_HY> GetIndustryTree()
        {
            var industries = SystemUtils.OracleEFDbContext.DM_GY_HY.Where(p => string.IsNullOrEmpty(p.SJHY_DM)).ToList();
            return OrderIndustry(industries);
        }

        private static List<DM_GY_HY> OrderIndustry(List<DM_GY_HY> industries)
        {
            industries = industries.OrderBy(p => p.HY_DM).ToList();
            foreach (var industry in industries)
            {
                if (industry.Children != null && industry.Children.Count > 0)
                {
                    industry.Children = OrderIndustry(industry.Children);
                }
            }
            return industries;
        }

        private static IQueryable<V_QYMCSS> GetSortBaseQuery(SortCondition condition)
        {
            // 入库年度
            var rknd = condition.SSND.ToString();
            var query = new OracleEFDbContext().V_QYMCSS.Where(p => p.RKND.Trim() == rknd);
            // 行业代码
            if (condition.HYDM != null && !condition.HYDM.Any(p => p == "all"))
            {
                var hys = condition.HYDM.Where(p => p.Length == 4).ToList();
                query = query.Where(p => hys.Contains(p.HY_DM));
            }
            // 企业规模
            if (condition.QYGM != 9)
            {

            }
            // 所在片区
            if (!string.IsNullOrEmpty(condition.SZPQ))
            {
                query = query.Where(p => p.SZPQ.Contains(condition.SZPQ));
            }
            // 主管税务局
            if (!string.IsNullOrEmpty(condition.ZGSWJ_DM) && condition.ZGSWJ_DM != "all")
            {
                query = query.Where(p => p.SWJG_DM == condition.ZGSWJ_DM);
            }

            if (condition.MJ_Min > 0)
            {
                query = query.Where(p => (p.ZYTDMJ + p.CUZTDMJ) * 0.0015 > condition.MJ_Min);
            }

            if (condition.MJ_Max > 0)
            {
                query = query.Where(p => (p.ZYTDMJ + p.CUZTDMJ) * 0.0015 < condition.MJ_Max);
            }
            return query;
        }

        public static SortResult GetSortData(SortCondition condition, bool newCondition)
        {
            if (newCondition || _SortResult == null)
            {
                var query = GetSortBaseQuery(condition);
                // 自身根据NSRBH去重
                var tmpRows = query.ToList();
                var rows = (from t in tmpRows
                            group t by t.NSRSBH into g
                            select g.FirstOrDefault()).ToList();
                var total = rows.Count;

                // 数据排序
                rows = rows.OrderByDescending(p => p.MCSS).ToList();

                // 计算总亩均
                // 总面积
                var tdmj = rows.Sum(p => p.ZYTDMJ + p.CUZTDMJ) * 0.0015;
                // 总税收
                var ssze = rows.Sum(p => p.YNSSZE);
                // 亩产均值
                var mcjz = (double)(tdmj == null || ssze == null || tdmj == 0 ? 0 : ssze / tdmj);
                mcjz = Math.Round(mcjz, 2, MidpointRounding.AwayFromZero);

                // 企业排名
                for (int i = 0, l = rows.Count; i < l; i++)
                {
                    rows[i].RN = i + 1;
                    rows[i].MCJZ = mcjz;
                    var mcjzb = (double)(rows[i].MCSS == null || mcjz == 0 ? 0 : rows[i].MCSS / mcjz);
                    rows[i].MCJZB = Math.Round(mcjzb, 2, MidpointRounding.AwayFromZero);
                }

                // 各项数据汇总
                var sumRow = (from r in rows
                              group r by 1 into g
                              select new V_QYMCSS()
                              {
                                  NSRMC = "合计",
                                  ZYYWSR = g.Sum(p => p.ZYYWSR),
                                  SSZE = g.Sum(p => p.SSZE),
                                  YNSSZE = g.Sum(p => p.YNSSZE),
                                  MCSS = mcjz,
                                  MCJZ = mcjz,
                                  MCJZB = 1,
                                  GS_QYSDS = g.Sum(p => p.GS_QYSDS),
                                  GS_ZZS = g.Sum(p => p.GS_ZZS),
                                  TDSYS = g.Sum(p => p.TDSYS),
                                  FCS = g.Sum(p => p.FCS),
                                  YYS = g.Sum(p => p.YYS),
                                  QYSDS = g.Sum(p => p.QYSDS),
                                  CJS = g.Sum(p => p.CJS),
                                  ZZS = g.Sum(p => p.ZZS),
                                  MDT = g.Sum(p => p.MDT),
                                  YHS = g.Sum(p => p.YHS),
                                  TDZZS = g.Sum(p => p.TDZZS),
                                  JYFFJ = g.Sum(p => p.JYFFJ),
                                  DFJYFFJ = g.Sum(p => p.DFJYFFJ),
                                  SLJSJJ = g.Sum(p => p.SLJSJJ),
                                  WHSYF = g.Sum(p => p.WHSYF),
                                  ZYTDMJ = g.Sum(p => p.ZYTDMJ),
                                  CUZTDMJ = g.Sum(p => p.CUZTDMJ),
                                  CENGZTDMJ = g.Sum(p => p.CENGZTDMJ)
                              }).ToList();

                SortResult sr = new SortResult()
                {
                    total = total,
                    rows = rows,
                    footer = sumRow
                };
                _SortResult = sr;
            }
            return _SortResult;
        }

        public static List<DM_GY_SWJG> GetDepartments()
        {
            var departmentStr = SystemUtils.Config.Departments.ToString();
            var department = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DM_GY_SWJG>>(departmentStr);
            return department;
            //return SystemUtils.OracleEFDbContext.DM_GY_SWJG.Where(p => p.SWJG_DM.StartsWith("23304") && p.SWJG_DM.EndsWith("0000")).OrderBy(p => p.SWJG_DM).ToList();
        }

        public static void ExcelVerify(Workbook workbook)
        {
            HomeUtils._Companies = null;
            HomeUtils._Errors = null;
            if (workbook == null || workbook.Worksheets.Count == 0) throw new Exception("上传文件不包含有效数据！");
            Worksheet ws = workbook.Worksheets[0];
            int rowCount = ws.Cells.Rows.Count;
            int columnCount = ws.Cells.Columns.Count;
            if (columnCount < 6) throw new Exception("数据列不完整！");

            var companies = new List<QYGM0>();
            var errors = new List<Error>();



            List<string> departmentIDs = (from p in HomeUtils.GetDepartments()
                                          select p.SWJG_DM).ToList();

            for (int i = 1; i < rowCount; i++)
            {
                Row row = ws.Cells.Rows[i];

                var NSRSBH = row[0].Value == null ? string.Empty : row[0].Value.ToString().Trim();
                var SHXYDM = row[1].Value == null ? string.Empty : row[1].Value.ToString().Trim();
                var QYMC = row[2].Value == null ? string.Empty : row[2].Value.ToString().Trim();
                var ZGSWJ_DM = row[3].Value == null ? string.Empty : row[3].Value.ToString().Trim();
                var SSND = row[4].Value == null ? string.Empty : row[4].Value.ToString().Trim();
                var QYGM = 0;

                var error = new Error();
                // 纳税人规则
                if (string.IsNullOrEmpty(NSRSBH))
                {
                    error.ErrorMessages.Add("纳税人识别号不能为空。");
                }
                else if (NSRSBH.Length > 20)
                {
                    error.ErrorMessages.Add(string.Format("纳税人识别号“<span class='error-field'>{0}</span>”长度过长。", NSRSBH));
                }
                // 社会信用代码
                if (SHXYDM.Length > 20)
                {
                    error.ErrorMessages.Add(string.Format("社会信用代码“<span class='error-field'>{0}</span>”长度过长。", SHXYDM));
                }

                // 企业名称
                if (string.IsNullOrEmpty(QYMC))
                {
                    error.ErrorMessages.Add("企业名称不能为空。");
                }
                else if (QYMC.Length > 300)
                {
                    error.ErrorMessages.Add(string.Format("企业名称“<span class='error-field'>{0}</span>”长度过长。", QYMC));
                }

                // 税务部门编码
                if (string.IsNullOrEmpty(ZGSWJ_DM))
                {
                    error.ErrorMessages.Add("税务部门编码不能为空。");
                }
                else if (!departmentIDs.Any(c => c == ZGSWJ_DM))
                {
                    error.ErrorMessages.Add(string.Format("税务部门编码“<span class='error-field'>{0}</span>”不是有效的编码。", ZGSWJ_DM));
                }

                // 税收年度
                int ssnd = 0;
                if (string.IsNullOrEmpty(SSND))
                {
                    error.ErrorMessages.Add("税收年度不能为空");
                }
                else if (!Int32.TryParse(SSND, out ssnd) || ssnd < 1990 || ssnd > 2050)
                {
                    error.ErrorMessages.Add(string.Format("税收年度“<span class='error-field'>{0}</span>”不是有效的4位数字。", SSND));
                }

                // 纳税人识别号、税务部门编码、税收年度是否重复
                if (SystemUtils.OracleEFDbContext.QYGM.Where(p => p.NSRSBH == NSRSBH).Where(p => p.ZGSWJ_DM == ZGSWJ_DM).Where(p => p.SSND == ssnd).Count() != 0)
                {
                    error.ErrorMessages.Add("数据库中已包含相同数据。");
                }

                var qygm = new QYGM0()
                {
                    INDEX = i,
                    NSRSBH = NSRSBH,
                    SHXYDM = SHXYDM,
                    NSRMC = QYMC,
                    ZGSWJ_DM = ZGSWJ_DM,
                    SSND = ssnd,
                    QYGM = QYGM
                };

                companies.Add(qygm);

                // 判断是否有错误
                if (error.ErrorMessages.Count > 0)
                {
                    error.Index = i;
                    error.Title = string.Format("<span class='error-field'>第{0}行</span>{1}（{2}）", i, QYMC, NSRSBH);
                    error.Company = qygm;
                    errors.Add(error);
                }
            }

            // 自身数据重复
            var selfCount = (from p in companies
                             group p by new { p.NSRSBH, p.SSND, p.ZGSWJ_DM } into g
                             where g.Count() > 1
                             select g).ToList();

            if (selfCount.Count > 0)
            {
                var error = new Error();
                error.Title = "自身数据重复";
                foreach (var group in selfCount)
                {
                    var erMsg = string.Format("纳税人识别号：{0}，税收年度：{1}，税务部门代码：{2}，组合重复！", group.Key.NSRSBH, group.Key.SSND, group.Key.ZGSWJ_DM);
                    error.ErrorMessages.Add(erMsg);
                }
                errors.Add(error);
            }

            HomeUtils._Companies = companies;
            HomeUtils._Errors = errors;
        }
    }
}