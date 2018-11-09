using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ClientDependency.Core.FileRegistration.Providers;
using ClientDependency.Core.Config;

namespace ClientDependency.Core.FileRegistration.Providers
{
    public class StandardRenderer : BaseRenderer
    {

        public const string DefaultName = "StandardRenderer";

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            // Assign the provider a default name if it doesn't have one
            if (string.IsNullOrEmpty(name))
                name = DefaultName;

            base.Initialize(name, config);
        }

        /// <summary>
        /// Override because we need to ensure the & is replaced with &amp; This is only required for this one w3c compliancy, the URL itself is a valid URL.
        /// </summary>
        /// <param name="allDependencies"></param>
        /// <param name="paths"></param>
        /// <param name="jsOutput"></param>
        /// <param name="jsPreloadOutput"></param>
        /// <param name="cssOutput"></param>
        /// <param name="cssPreloadOutput"></param>
        /// <param name="http"></param>
        public override void RegisterDependencies(
            List<IClientDependencyFile> allDependencies, 
            HashSet<IClientDependencyPath> paths, 
            out string jsOutput,
            out string jsPreloadOutput,
            out string cssOutput,
            out string cssPreloadOutput, 
            HttpContextBase http)
        {
            base.RegisterDependencies(allDependencies, paths, out jsOutput, out jsPreloadOutput, out cssOutput, out cssPreloadOutput, http);

            jsOutput = jsOutput.Replace("&", "&amp;");
            jsPreloadOutput = jsPreloadOutput.Replace("&", "&amp;");
            cssOutput = cssOutput.Replace("&", "&amp;");
            cssPreloadOutput = cssPreloadOutput.Replace("&", "&amp;");
        }

        protected override string RenderJsDependencies(IEnumerable<IClientDependencyFile> jsDependencies, HttpContextBase http, IDictionary<string, string> htmlAttributes)
        {
            if (!jsDependencies.Any())
                return string.Empty;

            var sb = new StringBuilder();

            if (http.IsDebuggingEnabled || !EnableCompositeFiles)
            {
                foreach (var dependency in jsDependencies)
                {
                    sb.Append(RenderSingleJsFile(dependency.FilePath, htmlAttributes));
                }
            }
            else if (DisableCompositeBundling)
            {
                foreach (var dependency in jsDependencies)
                {
                    RenderJsComposites(http, htmlAttributes, sb, Enumerable.Repeat(dependency, 1));
                }
            }
            else
            {
                RenderJsComposites(http, htmlAttributes, sb, jsDependencies);
            }

            return sb.ToString();
        }

        protected override string RenderJsPreloadDependencies(IEnumerable<IClientDependencyFile> jsPreloadDependencies, HttpContextBase http, IDictionary<string, string> htmlAttributes)
        {
            if (!jsPreloadDependencies.Any())
                return string.Empty;

            var sb = new StringBuilder();

            if (http.IsDebuggingEnabled || !EnableCompositeFiles)
            {
                foreach (var dependency in jsPreloadDependencies)
                {
                    sb.Append(RenderSingleJsPreloadFile(dependency.FilePath, htmlAttributes));
                }
            }
            else if (DisableCompositeBundling)
            {
                foreach (var dependency in jsPreloadDependencies)
                {
                    RenderJsPreloadComposites(http, htmlAttributes, sb, Enumerable.Repeat(dependency, 1));
                }
            }
            else
            {
                RenderJsPreloadComposites(http, htmlAttributes, sb, jsPreloadDependencies);
            }

            return sb.ToString();
        }

        protected override string RenderCssDependencies(IEnumerable<IClientDependencyFile> cssDependencies, HttpContextBase http, IDictionary<string, string> htmlAttributes)
        {
            if (!cssDependencies.Any())
                return string.Empty;

            var sb = new StringBuilder();

            if (http.IsDebuggingEnabled || !EnableCompositeFiles)
            {
                foreach (var dependency in cssDependencies)
                {
                    sb.Append(RenderSingleCssFile(dependency.FilePath, htmlAttributes));
                }
            }
            else if (DisableCompositeBundling)
            {
                foreach (var dependency in cssDependencies)
                {
                    RenderCssComposites(http, htmlAttributes, sb, Enumerable.Repeat(dependency, 1));
                }
            }
            else
            {
                RenderCssComposites(http, htmlAttributes, sb, cssDependencies);
            }

            return sb.ToString();
        }

        protected override string RenderCssPreloadDependencies(
            IEnumerable<IClientDependencyFile> cssPreloadDependencies,
            HttpContextBase http,
            IDictionary<string, string> htmlAttributes)
        {
            if (!cssPreloadDependencies.Any())
                return string.Empty;

            var sb = new StringBuilder();

            if (http.IsDebuggingEnabled || !EnableCompositeFiles)
            {
                foreach (var dependency in cssPreloadDependencies)
                {
                    sb.Append(RenderSingleCssPreloadFile(dependency.FilePath, htmlAttributes));
                }
            }
            else if (DisableCompositeBundling)
            {
                foreach (var dependency in cssPreloadDependencies)
                {
                    RenderCssPreloadComposites(http, htmlAttributes, sb, Enumerable.Repeat(dependency, 1));
                }
            }
            else
            {
                RenderCssPreloadComposites(http, htmlAttributes, sb, cssPreloadDependencies);
            }

            return sb.ToString();
        }

        protected override string RenderSingleJsFile(string js, IDictionary<string, string> htmlAttributes)
        {
            return string.Format(HtmlEmbedContants.ScriptEmbedWithSource, js, htmlAttributes.ToHtmlAttributes());
        }

        protected override string RenderSingleJsPreloadFile(string jsPreload, IDictionary<string, string> htmlAttributes)
        {
            return string.Format(HtmlEmbedContants.ScriptPreloadEmbedWithSource, jsPreload, htmlAttributes.ToHtmlAttributes());
        }

        protected override string RenderSingleCssFile(string css, IDictionary<string, string> htmlAttributes)
        {
            return string.Format(HtmlEmbedContants.CssEmbedWithSource, css, htmlAttributes.ToHtmlAttributes());
        }

        protected override string RenderSingleCssPreloadFile(string cssPreload, IDictionary<string, string> htmlAttributes)
        {
            return string.Format(HtmlEmbedContants.CssPreloadEmbedWithSource, cssPreload, htmlAttributes.ToHtmlAttributes());
        }
    }
}
