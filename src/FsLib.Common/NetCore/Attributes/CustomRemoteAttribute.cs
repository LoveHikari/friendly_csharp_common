using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetCore.Attributes
{
    /// <summary>
    /// Remote Attribute for Client an Server validation.
    /// https://stackoverflow.com/questions/5393020/remoteattribute-validator-does-not-fire-server-side
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class CustomRemoteAttribute : Microsoft.AspNetCore.Mvc.RemoteAttribute
    {
        /// <summary>
        /// List of all Controllers on MVC Application
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Type> GetControllerList()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(ControllerBase))).ToList();
        }

        /// <summary>
        /// Constructor of base class.
        /// </summary>
        protected CustomRemoteAttribute() { }

        /// <summary>
        /// Constructor of base class.
        /// </summary>
        public CustomRemoteAttribute(string routeName) : base(routeName) { }

        /// <summary>
        /// Constructor of base class.
        /// </summary>
        public CustomRemoteAttribute(string action, string controller) : base(action, controller) { }

        /// <summary>
        /// Constructor of base class.
        /// </summary>
        public CustomRemoteAttribute(string action, string controller, string areaName) : base(action, controller, areaName) { }

        /// <summary>
        /// Overridden IsValid function
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Find the controller passed in constructor
            var controller = GetControllerList().FirstOrDefault(x => x.Name == string.Format("{0}Controller", this.RouteData["controller"]));

            var otherController = validationContext.GetRequiredService(controller);
            controller = otherController.GetType();

            if (controller == null)
            {
                // Default behavior of IsValid when no controller is found.
                return ValidationResult.Success;
            }
            List<Type> proTypeList = new List<Type>() { value?.GetType()??typeof(object) };
            List<object> proList = new List<object>() { value };
            if (!string.IsNullOrWhiteSpace(AdditionalFields))
            {
                var fields = AdditionalFields.Split(',', StringSplitOptions.RemoveEmptyEntries);
                proTypeList.AddRange(validationContext.ObjectType.GetProperties().Where(info => fields.Any(s => info.Name==s)).Select(info => info.PropertyType));

                proList.AddRange(validationContext.ObjectType.GetProperties().Where(info => fields.Any(s => info.Name==s)).Select(info => info.GetValue(validationContext.ObjectInstance)));
            }
            // Find the Method passed in constructor
            var mi = controller.GetMethod(this.RouteData["action"].ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, proTypeList.ToArray(), null);

            if (mi == null)
            {
                // Default behavior of IsValid when action not found
                return ValidationResult.Success;
            }

            // invoke the method on the controller with value and "AdditionalFields"
            JsonResult result =(JsonResult)((Task<object>)mi.Invoke(otherController, proList.ToArray())).Result;
            //JsonResult result = (JsonResult)mi.Invoke(otherController, proList.ToArray());
            // Return success or the error message string from CustomRemoteAttribute
            string errorMessaqe = result.Value as string;
            if (errorMessaqe == null)
            {
                bool isValid;
                try
                {
                    isValid = (bool)result.Value;
                }
                catch (Exception)
                {
                    isValid = false;
                }
                return isValid ? ValidationResult.Success : new ValidationResult(base.ErrorMessageString);
            }
            else
                return new ValidationResult(errorMessaqe);
        }
    }
}