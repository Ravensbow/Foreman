//using Foreman.Shared.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Foreman.Shared.Filters
//{
//    public class IsCourseManager : IActionFilter
//    {
//        private Type modelType;
//        private ICourseService courseService;

//        public IsCourseManager(Type type, ICourseService courseService)
//        {
//            modelType = type;
//            this.courseService = courseService;
//        }

//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//            return;
//        }

//        public void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var model = filterContext.ActionArguments.Values.FirstOrDefault(x => x?.GetType() == modelType);
//            int? courseId = (modelType.GetProperty("CourseId").GetValue(model, null) as int?);
//            int? categoryId = courseId != null ? courseService.GetCategoryId(courseId.Value) : null;//Global.Container.GetInstance<ICourseService>();
//            if (!courseId.HasValue || (!filterContext.HttpContext.User.HasClaim("CourseManager", courseId.ToString()) && !filterContext.HttpContext.User.HasClaim("CategoryManager", courseId.ToString())))
//            {
//                filterContext.Result = null;//new ForbidResult("Access to course management is forbidden for your user role!");
//            }
//        }
//    }
//}
