using HtmlTags;

namespace WebApplicationMVC.Models {
    public class HTMLgenerator<T> {

        public string GenerateTable(List<T> list) {
            if(list.Count == 0) {
                throw new ArgumentException("Provided list is empty");
            }

            string[] headerNames = list.FirstOrDefault()?.GetType().GetProperties().Select(prop => prop.Name).ToArray();

            var table = new TableTag();

            //create headers for table rows
            table.AddHeaderRow(row => {
                foreach (string headerName in headerNames) {
                    row.Header(headerName);
                }
            });

            foreach(var item in list) {
                table.AddBodyRow(row => {
                    for(int i = 0; i < headerNames.Length; i++) {

                        //get the property from the object
                        var propInfo = item.GetType().GetProperty(headerNames[i]);

                        //see if the property exists
                        if (propInfo != null) {
                            //try and get the stored value
                            var propValue = propInfo.GetValue(item);
                            //if it has one, place it if not leave blank
                            row.Cell(propValue?.ToString() ?? "");
                        }
                        else {
                            //No value in the property, leave blank
                            row.Cell("");
                        }
                    }
                });
            }
            //return finished table
            return table.ToHtmlString();
        }

    }

    //var table = new TableTag();
    //table.AddClass("table table-boardered");

    ////add row headers to match the prexisting table format
    //table.AddHeaderRow(row => {
    //    row.Header("Booking id");
    //    row.Header("Noter");
    //    row.Header("Start tidspunkt");
    //    row.Header("Slut tidspunkt");
    //});

    ////insert the data from the booking history items
    //foreach (BookingHistoryItem item in bhi) {
    //    //mark rows with bookings that are older than current date with red.
    //    if (item.TimeEnd < DateTime.Now) {
    //        table.AddBodyRow(row => {
    //            row.Cell(item.Id + "").Attr("class", "text-danger text-lg");
    //            row.Cell(item.Notes).Attr("class", "text-danger text-lg");
    //            row.Cell(item.TimeStart.ToString()).Attr("class", "text-danger text-lg");
    //            row.Cell(item.TimeEnd.ToString()).Attr("class", "text-danger text-lg");
    //        });
    //    }
    //    else {
    //        table.AddBodyRow(row => {
    //            row.Cell(item.Id + "");
    //            row.Cell(item.Notes);
    //            row.Cell(item.TimeStart.ToString());
    //            row.Cell(item.TimeEnd.ToString());
    //        });
    //    }
    //}

    //result = Content(table.ToHtmlString(), "text/html");
}
