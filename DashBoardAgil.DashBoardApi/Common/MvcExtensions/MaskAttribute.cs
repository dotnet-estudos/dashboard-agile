using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComprasNet.Common.MvcExtensions
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaskAttribute : Attribute, IMetadataAware
    {
        private readonly string _atributo;
        private readonly string _mask;

        public MaskAttribute(string mask, string atributo)
        {
            _mask = mask;
            _atributo = atributo;
        }

        private const string ScriptText = "<script type='text/javascript'> $(document).ready(function () {{" +
                                           "  ConfigurarMascara('{0}','{1}'); }});" +
                                           "</script>";

        public const string templateHint = "_maskedInput";

        private int _count;

        public string Id
        {
            get { return "maskedInput_" + _count; }
        }

        internal HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var list = Context.Items["Scripts"] as IList<string> ?? new List<string>();
            _count = list.Count;
            metadata.TemplateHint = templateHint;
            metadata.AdditionalValues[templateHint] = Id;

            string script = string.Format(ScriptText, _atributo, _mask);

            if (!list.Any(x => x.Equals(script)))
            {
                list.Add(script);
            }

            Context.Items["Scripts"] = list;
        }
    }
}
