using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebForms.Combobox.Controls;

namespace WebForms.Combobox.Web
{
    public partial class _Default : Page
    {
        private static Dictionary<string, List<Result>> mock = new Dictionary<string, List<Result>>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ItemsRequested(object sender, ItemsRequestedEventArgs e)
        {
            if (!mock.ContainsKey(e.Text))
            {
                var total = new Random().Next(5, 500);
                mock[e.Text] = Enumerable.Range(0, total)
                          .Select(i => new Result {Id = i, Name = string.Format("{0}-{1}", e.Text, i)})
                          .ToList();
            }
            var results = mock[e.Text];
            e.Total = results.Count;
            e.Result = results.Skip(e.Page*e.PageSize).Take(e.PageSize);
        }

        private class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        protected void Postback(object sender, EventArgs e)
        {
            if (combo.Selected != null)
            {
                text.Text = combo.Selected.Name;
            }
        }
    }
}