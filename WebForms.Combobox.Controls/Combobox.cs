using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebForms.Combobox.Controls
{
    public class Combobox : CompositeControl, IScriptControl, ICallbackEventHandler
    {
        private ItemsRequestedEventArgs result;

        private readonly HtmlContainerControl content;
        private readonly HiddenField selectedField;
        private Selected selected;

        public Combobox()
        {
            content = new HtmlGenericControl("div");
            selectedField = new HiddenField();
        }

        protected override void CreateChildControls()
        {
            content.Attributes["class"] = "combobox";
            content.Attributes["data-bind"] = "combobox: options, comboboxValue: selected";
            content.ID = "content";
            selectedField.ID = "selected";

            Controls.Add(content);
            Controls.Add(selectedField);
        }

        protected override void OnPreRender(EventArgs e)
        {
            InitScripts();
            AddCss();

            base.OnPreRender(e);
        }

        private void AddCss()
        {
            var id = "comboboxCss";

            if (Page.Header.FindControl(id) == null)
            {
                var cssMetaData = new HtmlGenericControl("link");
                cssMetaData.ID = id;
                cssMetaData.Attributes.Add("rel", "stylesheet");
                cssMetaData.Attributes.Add("href",
                    Page.ClientScript.GetWebResourceUrl(typeof(Combobox),
                    "WebForms.Combobox.Controls.Content.knockout.combobox-1.0.71.0.min.css"));
                cssMetaData.Attributes.Add("type", "text/css");
                cssMetaData.Attributes.Add("media", "screen");
                Page.Header.Controls.Add(cssMetaData);
            }
        }

        private void InitScripts()
        {
            ScriptManager.GetCurrent(Page).RegisterScriptControl(this);


            ScriptManager.RegisterStartupScript(this,
                typeof(Combobox),
                "init" + UniqueID,
                string.Format(
                    @"$(document).ready(function() {{ new ComboboxWrapper('{0}', '{1}', '{2}'); }});",
                    UniqueID,
                    content.ClientID,
                    selectedField.ClientID),
                true);

            Page.ClientScript.GetCallbackEventReference(
                this,
                "",
                "this.callBack",
                null,
                null,
                false);

        }

        IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
        {
            var assembly = "WebForms.Combobox.Controls";
            return new[]
                {
                    new ScriptReference("WebForms.Combobox.Controls.Scripts.knockout.combobox-1.0.71.0.min.js", assembly),
                    new ScriptReference("WebForms.Combobox.Controls.JS.combobox.js", assembly)
                };
        }

        IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
        {
            return new ScriptDescriptor[] { new ScriptControlDescriptor("WebForms.Combobox.Controls.Combobox", ClientID) };
        }

        public event EventHandler<ItemsRequestedEventArgs> ItemsRequested;

        protected virtual void OnItemsRequested(object sender, ItemsRequestedEventArgs e)
        {
            if (ItemsRequested != null)
                ItemsRequested(sender, e);
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            var args = JsonConvert.DeserializeObject<ItemsRequestedEventArgs>(eventArgument);
            OnItemsRequested(this, args);
            result = args;
        }

        public string GetCallbackResult()
        {
            return JsonConvert.SerializeObject(new { total = result.Total, data = result.Result });
        }

        public Selected Selected
        {
            get { return selected = selected ?? GetSelected(); }
            set
            {
                if (value == null)
                    selectedField.Value = null;
                else
                {
                    selected = value;
                    selectedField.Value = JsonConvert.SerializeObject(value);
                }
            }
        }

        private Selected GetSelected()
        {
            if (!string.IsNullOrEmpty(selectedField.Value))
            {
                return JsonConvert.DeserializeObject<Selected>(selectedField.Value);
            }
            return null;
        }
    }


    public class Selected
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemsRequestedEventArgs : EventArgs
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Text { get; set; }
        public int Total { get; set; }
        public object Result { get; set; }
    }
}