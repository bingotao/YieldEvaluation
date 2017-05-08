using Aspose.Cells;
using JXGIS.Common.BaseLib;
using JXGIS.Common.Entity;
using JXGIS.YieldEvaluation.Business;
using JXGIS.YieldEvaluation.Web.Attributes;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace JXGIS.YieldEvaluation.Web.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [AdminAuthorize]
        public ActionResult EnterpriseSortPage()
        {
            return View();
        }

        [AdminAuthorize]
        public ActionResult DataProcess(int sjnf, string jgms, string swbm, string dsbm)
        {
            ReturnObject ro = null;

            DbCommand cmd = null;
            try
            {
                User user = LoginUtils.GetUser();

                if (user == null || user.UserName.ToLower() != "admin")
                {
                    throw new Exception("没有数据加工权限！");
                }

                var dbContext = SystemUtils.OracleEFDbContext;
                OracleParameter nyear = new OracleParameter("nyear", OracleDbType.Int32, ParameterDirection.Input);
                nyear.Value = sjnf;
                OracleParameter qyid = new OracleParameter("qyid", OracleDbType.Varchar2, ParameterDirection.Input);
                qyid.Value = dsbm;
                OracleParameter swbmbm = new OracleParameter("swbmbm", OracleDbType.Varchar2, ParameterDirection.Input);
                swbmbm.Value = swbm;

                cmd = dbContext.Database.Connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                switch (jgms)
                {
                    case "全库":
                        cmd.CommandText = "SJJG_MCSS_QK";
                        cmd.Parameters.Add(nyear);
                        //SystemUtils.OracleEFDbContext.Database.ExecuteSqlCommand("SJJG_MCSS_QK", nyear);
                        break;
                    case "单户":
                        cmd.CommandText = "SJJG_MCSS_DH";
                        cmd.Parameters.Add(nyear);
                        cmd.Parameters.Add(qyid);
                        //SystemUtils.OracleEFDbContext.Database.ExecuteSqlCommand("SJJG_MCSS_DH", nyear, qyid);
                        break;
                    case "税务部门":
                        cmd.CommandText = "SJJG_MCSS_BM";
                        cmd.Parameters.Add(nyear);
                        cmd.Parameters.Add(swbmbm);
                        //SystemUtils.OracleEFDbContext.Database.ExecuteSqlCommand("SJJG_MCSS_BM", nyear, swbmbm);
                        break;
                    default:
                        throw new Exception("未知的加工模式！");
                }
                cmd.Connection.Open();
                var count = cmd.ExecuteNonQueryAsync();
                cmd.Connection.Close();

                ro = new ReturnObject();
                ro.Data = "正在加工中，请稍后查看加工结果！";
            }
            catch (Exception ex)
            {
                if (cmd != null) cmd.Connection.Close();
                ro = new ReturnObject(ex);
            }

            return Json(ro);
        }

        [AdminAuthorize]
        public ActionResult GetSortData(int page, int rows, bool newCondition, string sortField = "MCSS", bool desc = true)
        {
            ReturnObject ro = null;
            try
            {
                var condition = GetSortCondition();
                var sortResult = HomeUtils.GetSortData(condition, newCondition);

                var rowsData = sortResult.rows;
                List<V_QYMCSS> finalRows = null;

                finalRows =
                    desc ?
                    rowsData.OrderByDescending(p => p[sortField]).Skip((page - 1) * rows).Take(rows).ToList() :
                    rowsData.OrderBy(p => p[sortField]).Skip((page - 1) * rows).Take(rows).ToList();

                ro = new ReturnObject(new SortResult()
                {
                    rows = finalRows,
                    footer = sortResult.footer,
                    total = sortResult.total
                });
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }
            string rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro, new CustomNullableDoubleConverter(), new CustomDoubleConverter());
            return Content(rs);
        }

        [AdminAuthorize]
        public ActionResult ExportSortExcel()
        {
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            ws.Name = "亩产税收详情";
            var condition = GetSortCondition();
            var data = HomeUtils.GetSortData(condition, false);
            var rows = data.rows.ToList();
            var footerRows = data.footer as List<V_QYMCSS>;
            rows.AddRange(footerRows);

            var fields = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExportField>>(SystemUtils.Config.SortExcelExportFields.ToString());
            Aspose.Cells.Style styleHeader = wb.Styles[wb.Styles.Add()];
            styleHeader.Pattern = Aspose.Cells.BackgroundType.Solid;
            styleHeader.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            styleHeader.ForegroundColor = System.Drawing.Color.FromArgb(240, 240, 240);
            styleHeader.Font.IsBold = true;
            styleHeader.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            Aspose.Cells.Style styleData = wb.Styles[wb.Styles.Add()];
            styleData.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleData.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleData.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleData.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            // 写入表头
            for (int i = 0, l = fields.Count; i < l; i++)
            {
                var field = fields[i];
                ws.Cells[0, i].PutValue(field.Alias);
                ws.Cells[0, i].SetStyle(styleHeader);
            }
            // 写入数据
            for (int i = 0, l = rows.Count; i < l; i++)
            {
                var row = rows[i];
                for (int j = 0; j < fields.Count; j++)
                {
                    var field = fields[j];
                    var value = row[field.Field];
                    ws.Cells[i + 1, j].PutValue(value);
                    ws.Cells[i + 1, j].SetStyle(styleData);
                }
            }

            ws.AutoFitColumns();
            MemoryStream ms = new MemoryStream();
            wb.Save(ms, SaveFormat.Excel97To2003);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "亩产税收详情.xls");
        }

        [AdminAuthorize]
        public ActionResult ClearUploadDatas()
        {
            ReturnObject ro = null;

            try
            {
                HomeUtils._Companies = null;
                HomeUtils._Errors = null;
                ro = new ReturnObject();
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(ro));
        }

        [AdminAuthorize]
        public ActionResult GetIndustryTree()
        {
            ReturnObject ro = null;
            try
            {
                //var industryTree = HomeUtils.GetIndustryTree();
                ro = new ReturnObject(SystemUtils.Config.Industries);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }
            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
        [AdminAuthorize]
        public ActionResult GetDepartments()
        {
            ReturnObject ro = null;
            try
            {
                //var departments = HomeUtils.GetDepartments();
                var departments = SystemUtils.Config.Departments;
                ro = new ReturnObject(departments);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }
            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
        [AdminAuthorize]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            ReturnObject ro = null;

            try
            {
                Stream fs = file.InputStream;
                Workbook wb = new Workbook(fs);
                HomeUtils.ExcelVerify(wb);

                ro = new ReturnObject();
                ro.AddData("companies", HomeUtils._Companies == null ? new List<QYGM0>() : HomeUtils._Companies.Take(10));
                ro.AddData("count", HomeUtils._Companies == null ? 0 : HomeUtils._Companies.Count);
                ro.AddData("errors", HomeUtils._Errors ?? new List<Error>());
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }
            string rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
        [AdminAuthorize]
        public ActionResult ImportQYGM()
        {
            ReturnObject ro = null;
            try
            {
                if (HomeUtils._Errors.Count > 0)
                {
                    throw new Exception("数据包含错误信息，请先检查数据！");
                }

                if (HomeUtils._Companies == null && HomeUtils._Companies.Count == 0)
                {
                    throw new Exception("无可导入数据！");
                }

                using (TransactionScope ts = new TransactionScope())
                {
                    var dbContext = SystemUtils.OracleEFDbContext;
                    dbContext.QYGM.AddRange(HomeUtils._Companies);
                    dbContext.SaveChanges();
                    ts.Complete();
                    HomeUtils._Companies = null;
                }
                ro = new ReturnObject(true);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }

            string rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
        [AdminAuthorize]
        public ActionResult GetPageCompanies(int pageSize, int pageNumber)
        {
            ReturnObject ro = null;

            try
            {
                ro = new ReturnObject();
                ro.AddData("companies", HomeUtils._Companies != null ? HomeUtils._Companies.Skip((pageNumber - 1) * pageSize).Take(pageSize) : new List<QYGM0>());
                ro.AddData("count", HomeUtils._Companies != null ? HomeUtils._Companies.Count : 0);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }
            string rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
        [AdminAuthorize]
        public ActionResult UpSortCondition(SortCondition sortCondition)
        {
            ReturnObject ro = null;
            try
            {
                ClearSortCondition();
                SetSortCondition(sortCondition);
                ro = new ReturnObject(true);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }
            string rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }
        [AdminAuthorize]
        private void SetSortCondition(SortCondition sortCondition)
        {
            GetSession()["SortCondition"] = sortCondition;
        }
        [AdminAuthorize]
        private void ClearSortCondition()
        {
            var session = GetSession();
            if (GetSession()["SortCondition"] != null)
            {
                GetSession()["SortCondition"] = null;
            }
        }
        [AdminAuthorize]
        private HttpSessionState GetSession()
        {
            return System.Web.HttpContext.Current.Session;
        }

        [AdminAuthorize]
        private SortCondition GetSortCondition()
        {
            return GetSession()["SortCondition"] as SortCondition;
        }

        [AdminAuthorize]
        public ActionResult GetQYGM(int ssnd, string nsrsbh, string nsrmc, string zgswj_dm, int page, int rows)
        {
            var query = SystemUtils.OracleEFDbContext.QYGM.AsNoTracking().Where(p => p.SSND == ssnd);
            if (!string.IsNullOrWhiteSpace(nsrsbh)) query = query.Where(p => p.NSRSBH.Contains(nsrsbh));
            if (!string.IsNullOrWhiteSpace(nsrmc)) query = query.Where(p => p.NSRMC.Contains(nsrmc));
            if (zgswj_dm != "all") query = query.Where(p => p.ZGSWJ_DM == zgswj_dm);

            var data = query.OrderBy(p => p.SSND).Skip((page - 1) * rows).Take(rows).ToList();
            var count = query.Count();
            Dictionary<string, object> rt = new Dictionary<string, object>();
            rt.Add("rows", data);
            rt.Add("total", count);

            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(rt);
            return Content(rs);
        }

        [AdminAuthorize]
        public ActionResult SaveQYGM(QYGM0 oldEntity, QYGM0 newEntity)
        {
            ReturnObject ro = null;
            try
            {
                var error = string.Empty;
                if (string.IsNullOrEmpty(newEntity.NSRSBH)) error = "纳税人识别号不能为空！";
                if (string.IsNullOrEmpty(newEntity.ZGSWJ_DM)) error = "税务部门编码不能为空！";
                if (!string.IsNullOrEmpty(error)) throw new Exception(error);

                using (TransactionScope ts = new TransactionScope())
                {
                    // 新增
                    if (oldEntity.NSRSBH == null)
                    {
                        var query = SystemUtils.OracleEFDbContext.QYGM.Where(p => p.NSRSBH == newEntity.NSRSBH)
                                    .Where(p => p.SSND == newEntity.SSND)
                                    .Where(p => p.ZGSWJ_DM == newEntity.ZGSWJ_DM);

                        var count = query.Count();
                        if (count > 0) throw new Exception("数据库中已经包含该数据！");
                        SystemUtils.OracleEFDbContext.QYGM.Add(newEntity);
                    }
                    else
                    {
                        var query = SystemUtils.OracleEFDbContext.QYGM.Where(p => p.NSRSBH == oldEntity.NSRSBH)
                                    .Where(p => p.SSND == oldEntity.SSND)
                                    .Where(p => p.ZGSWJ_DM == oldEntity.ZGSWJ_DM);
                        var count = query.Count();
                        if (count == 0) throw new Exception("数据项已被更新，请重新查询并编辑！");

                        var qygm = query.First();
                        SystemUtils.OracleEFDbContext.QYGM.Remove(qygm);
                        SystemUtils.OracleEFDbContext.QYGM.Add(newEntity);
                    }
                    SystemUtils.OracleEFDbContext.SaveChanges();
                    ts.Complete();
                }
                ro = new ReturnObject(true);
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }

            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }

        [AdminAuthorize]
        public ActionResult DeleteQYGM(QYGM0 entity)
        {
            ReturnObject ro = null;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var items = SystemUtils.OracleEFDbContext.QYGM
                        .Where(p => p.NSRSBH == entity.NSRSBH)
                        .Where(p => p.SSND == entity.SSND)
                        .Where(p => p.ZGSWJ_DM == entity.ZGSWJ_DM).ToList();
                    SystemUtils.OracleEFDbContext.QYGM.RemoveRange(items);
                    SystemUtils.OracleEFDbContext.SaveChanges();
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                ro = new ReturnObject(ex);
            }

            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(ro);
            return Content(rs);
        }

        [AdminAuthorize]
        public ActionResult GetPLOCK()
        {
            Dictionary<string, object> rt = new Dictionary<string, object>();
            var rows = SystemUtils.OracleEFDbContext.SYS_PLOCK.ToList();
            rt.Add("rows", rows);
            return Json(rt);
        }

        public ActionResult GetSJJGLS()
        {
            Dictionary<string, object> rt = new Dictionary<string, object>();
            var rows = SystemUtils.OracleEFDbContext.SJJGLS.OrderByDescending(p => p.LOGDATE).Take(20).ToList().OrderBy(p => p.LOGDATE);
            rt.Add("rows", rows);
            var rs = Newtonsoft.Json.JsonConvert.SerializeObject(rows,new CustomDateTimeConverter("yyyy年MM月dd日HH时mm分ss秒"));
            return Content(rs);
        }
    }
}