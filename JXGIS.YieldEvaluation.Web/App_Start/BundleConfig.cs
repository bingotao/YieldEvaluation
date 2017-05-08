//using JXGIS.Common.BaseLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace JXGIS.YieldEvaluation.Web
//{
//    public class BundleConfig
//    {
//        private static string _refPath = "~/Reference/";
//        private static string _viewPath = "~/Views/";
//        private static string _cmpPath = "~/Extends/Components/";

//        public static void RegisterBundles(BundleCollection bundles)
//        {
//            #region 全局
//            //全局com js
//            bundles.Add(new ScriptBundle("~/gComScripts").IncludeDirectory("~/Extends/CommonJS", "*.js", true));

//            //全局components
//            bundles.Add(new BabelBundle("~/gBabels")
//                   .IncludeDirectory("~/Extends/Base", "*.jsx", true)
//                   .IncludeDirectory(_cmpPath + "Common", "*.jsx", true));

//            bundles.Add(new LessBundle("~/gLess")
//                   .Include("~/Extends/globalStyle.less")
//                   .IncludeDirectory("~/Extends/Base", "*.less", true)
//                   .IncludeDirectory(_cmpPath + "Common", "*.less", true));
//            #endregion

//            #region Home
//            //Index
//            bundles.Add(new LessBundle("~/home/index/css")
//                .IncludeDirectory(_cmpPath + "Home", "*.less", true)
//                .Include(_viewPath + "Home/css/Index.less"));
//            bundles.Add(new BabelBundle("~/home/index/js")
//                .IncludeDirectory(_cmpPath + "Home", "*.jsx", true)
//                .Include(_viewPath + "Home/js/Index.jsx"));

//            #endregion
//        }
//    }
//}