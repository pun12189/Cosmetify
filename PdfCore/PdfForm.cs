using Cosmetify.Model;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;

namespace Cosmetify.PdfCore
{
    public class PdfForm
    {
        /// <summary>
        /// The MigraDoc document that represents the invoice.
        /// </summary>
        Document _document;

        /// <summary>
        /// The text frame of the MigraDoc document that contains the address.
        /// </summary>
        TextFrame _addressFrame;

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        Table _table;

        /// <summary>
        /// The Represents the orientation od MigraDoc document.
        /// </summary>
        PageSetup pageSetup;

        /// <summary>
        /// Creates the invoice document.
        /// </summary>
        public Document CreateDocument(BatchModel batchModel)
        {
            // Create a new MigraDoc document.
            _document = new Document();
            _document.Info.Title = "Batch Order";
            _document.Info.Subject = "Details of Customer's Batch Order";
            _document.Info.Author = "COSMETIFY";
            pageSetup = _document.DefaultPageSetup.Clone();
            pageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;
            pageSetup.PageFormat = PageFormat.A4;
            pageSetup.LeftMargin = "1.5cm";
            pageSetup.RightMargin = "1.5cm";

            DefineStyles();

            CreatePage(batchModel);

            FillContent(batchModel);

            return _document;
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            var style = _document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Segoe UI";

            style = _document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("18cm", TabAlignment.Right);

            style = _document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("9cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal.
            style = _document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Segoe UI Semilight";
            style.Font.Size = 9;

            // Create a new style called Title based on style Normal.
            style = _document.Styles.AddStyle("Title", "Normal");
            style.Font.Name = "Segoe UI Semibold";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal.
            style = _document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "1mm";
            //style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("18cm", TabAlignment.Right);
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>
        void CreatePage(BatchModel batchModel)
        {
            // Each MigraDoc document needs at least one section.
            var section = _document.AddSection();

            // Define the page setup. We use an image in the header, therefore the
            // default top margin is too small for our invoice.
            section.PageSetup = pageSetup;
            // We increase the TopMargin to prevent the document body from overlapping the page header.
            // We have an image of 3.5 cm height in the header.
            // The default position for the header is 1.25 cm.
            // We add 0.5 cm spacing between header image and body and get 5.25 cm.
            // Default value is 2.5 cm.
            section.PageSetup.TopMargin = "4.25cm";

            // Put the logo in the header.
            var image = section.Headers.Primary.AddImage("./Extras/cosmetify.png");
            image.Height = "1.5cm";
            image.LockAspectRatio = false;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Center;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;

            // Create the footer.
            var paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("COSMETIFY ● PLOT NO 396 ● Industrial Area ● Phase-1 ● Panchkula");
            paragraph.Format.Font.Size = 12;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address.
            _addressFrame = section.AddTextFrame();
            _addressFrame.Height = "7.0cm";
            _addressFrame.Width = "7.0cm";
            _addressFrame.Left = ShapePosition.Left;
            _addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            _addressFrame.Top = "2.0cm";
            _addressFrame.RelativeVertical = RelativeVertical.Page;

            // Show the sender in the address frame.
            paragraph = _addressFrame.AddParagraph("Batch ID: " + batchModel.BatchOrderNo);
            //paragraph.Format.Font.Size = 12;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceAfter = 3;            

            paragraph = section.AddParagraph("Creation Date: " + batchModel.BatchDate.ToShortDateString());
            paragraph.Format.Font.Size = 8;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.LineSpacing = "1cm";
            paragraph.Format.Alignment = ParagraphAlignment.Right;

            if (batchModel.PlanningDate.Date >= DateTime.Now.Date)
            {
                paragraph = section.AddParagraph("Processed Date: " + batchModel.PlanningDate.ToShortDateString());
                paragraph.Format.Font.Size = 8;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.LineSpacing = "1cm";
                paragraph.Format.Alignment = ParagraphAlignment.Right;
            }

            if (batchModel.CompletionDate.Date >= DateTime.Now.Date && batchModel.CompletionDate.Date >= batchModel.PlanningDate.Date)
            {
                paragraph = section.AddParagraph("Completion Date: " + batchModel.CompletionDate.ToShortDateString());
                paragraph.Format.Font.Size = 8;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.LineSpacing = "1cm";
                paragraph.Format.Alignment = ParagraphAlignment.Right;
            }           

            paragraph = section.AddParagraph();
            paragraph.Format.LineSpacing = "2.5cm";
            paragraph.Format.LineSpacingRule = LineSpacingRule.Exactly;

            paragraph = section.AddParagraph("Raw Material Requisition Slip");
            paragraph.Format.Font.Underline = Underline.Single;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 12;

            if (!string.IsNullOrEmpty(batchModel.BatchOrderCollection[0].MFName))
            {
                paragraph = section.AddParagraph("Formulation Name: " + batchModel.BatchOrderCollection[0].MFName);
                // We use an empty paragraph to move the first text line below the address field.
                //paragraph.Format.Font.Size = 12;
                paragraph.Format.Font.Bold = true;
                paragraph.Format.LineSpacing = "1.25cm";
            }            

            paragraph = section.AddParagraph("Batch Size: " + batchModel.BatchOrderCollection[0].BatchSize + " " + batchModel.BatchOrderCollection[0].Units);
            // We use an empty paragraph to move the first text line below the address field.
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            //paragraph.Format.Font.Size = 10;
            paragraph.Format.LineSpacing = "1.25cm";            

            paragraph = section.AddParagraph("Packaging Type: " + batchModel.PkgType);
            //paragraph.Format.Font.Size = 8;
            paragraph.Style = "Reference";
            paragraph.AddTab();
            paragraph.AddText("Mfg Date: " + batchModel.MfgDate.ToShortDateString());

            paragraph = section.AddParagraph("Order Quantity: " + batchModel.PkgOrderQuantity);
            //paragraph.Format.Font.Size = 8;
            paragraph.Style = "Reference";
            paragraph.AddTab();
            paragraph.AddText("Expiry Date: " + batchModel.Expiry.ToShortDateString());

            paragraph = section.AddParagraph("Brand Name: " + batchModel.ProductName);
            paragraph.Format.LineSpacing = "1.25cm";
            paragraph.Format.SpaceBefore = "0.5mm";
            //paragraph.Format.Font.Size = 10;
            //paragraph = section.AddParagraph();
            // We use an empty paragraph to move the first text line below the address field.
            //paragraph.Format.LineSpacing = "1.25cm";
            //paragraph.Format.LineSpacingRule = LineSpacingRule.Exactly;
            // Add the print date field.

            //paragraph.Format.LineSpacingRule = LineSpacingRule.Exactly;
            // We use an empty paragraph to move the first text line below the address field.
            //paragraph.Format.LineSpacing = "1.25cm";
            //paragraph.Format.LineSpacingRule = LineSpacingRule.Exactly;
            if (!string.IsNullOrEmpty(batchModel.Description))
            {
                paragraph = section.AddParagraph();
                paragraph.AddFormattedText("Description: " + batchModel.Description);
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                //paragraph.Format.Font.Size = 8;
                // We use an empty paragraph to move the first text line below the address field.
                paragraph.Format.SpaceAfter = "2mm";
            }
            
            //paragraph.Format.LineSpacingRule = LineSpacingRule.Exactly;
            
            // And now the paragraph with text.
            /*paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = 1;
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("BRAND NAME: " + batchModel.ProductName, TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Created : " + batchModel.BatchDate);*/

            // Create the item table.
            _table = section.AddTable();
            _table.Style = "Table";
            _table.Borders.Color = TableBorder;
            _table.Borders.Width = 0.25;
            _table.Borders.Left.Width = 0.5;
            _table.Borders.Right.Width = 0.5;
            _table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns.
            var column = _table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = _table.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table.
            var row = _table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;
            row.Cells[0].AddParagraph("S.no");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].AddParagraph("Active Name");
            row.Cells[1].Format.Font.Bold = true;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[2].AddParagraph("Short Code");
            row.Cells[2].Format.Font.Bold = true;
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[3].AddParagraph("% Required");
            row.Cells[3].Format.Font.Bold = true;
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[4].AddParagraph("Stock Required");
            row.Cells[4].Format.Font.Bold = true;
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[5].AddParagraph("Remarks");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;

            _table.SetEdge(0, 0, 6, 1, Edge.Interior, BorderStyle.Single, 1, Color.FromRgb(163, 164, 173));
        }

        /// <summary>
        /// Creates the dynamic parts of the invoice.
        /// </summary>
        void FillContent(BatchModel batchModel)
        {
            // Fill the address in the address text frame.
            var paragraph = _addressFrame.AddParagraph();
            paragraph.AddText(batchModel.Customer.FirstName + " " + batchModel.Customer.LastName);
            paragraph.AddLineBreak();
            paragraph.AddText(batchModel.Customer.Address + ", " + batchModel.Customer.City);
            paragraph.AddLineBreak();
            paragraph.AddText(batchModel.Customer.State + ", " + batchModel.Customer.Country + "-" + batchModel.Customer.PinCode);
            paragraph.AddLineBreak();
            paragraph.AddText("Contact: " + batchModel.Customer.PhoneNumber);
            if (!string.IsNullOrEmpty(batchModel.Customer.EmailId))
            {
                paragraph.AddLineBreak();
                paragraph.AddText("Email ID: " + batchModel.Customer.EmailId);
            }

            int count = 0;
            foreach (var batchOrderModel in batchModel.BatchOrderCollection)
            {
                var row1 = this._table.AddRow();
                row1.Cells[0].AddParagraph((++count).ToString());
                row1.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[1].AddParagraph(batchOrderModel.Actives.ActivesName);
                row1.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[2].AddParagraph(batchOrderModel.Actives.ShortCode);
                row1.Cells[2].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[3].AddParagraph(batchOrderModel.PercentageRequired.ToString() + "%");
                row1.Cells[3].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[4].AddParagraph(batchOrderModel.StocksRequired.ToString() + batchOrderModel.Units);
                row1.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                row1.Cells[5].AddParagraph();
                row1.Cells[5].Format.Alignment = ParagraphAlignment.Center;
            }
            
            _table.SetEdge(0, _table.Rows.Count - 1, 6, 1, Edge.Box, BorderStyle.Single, 1);
            /*while (iter.MoveNext())
            {
                item = iter.Current;
                var quantity = GetValueAsDouble(item, "quantity");
                var price = GetValueAsDouble(item, "price");
                var discount = GetValueAsDouble(item, "discount");

                // Each item fills two rows.
                var row1 = this._table.AddRow();
                var row2 = this._table.AddRow();
                row1.TopPadding = 1.5;
                row1.Cells[0].Shading.Color = TableGray;
                row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                row1.Cells[0].MergeDown = 1;
                row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
                row1.Cells[1].MergeRight = 3;
                row1.Cells[5].Shading.Color = TableGray;
                row1.Cells[5].MergeDown = 1;

                row1.Cells[0].AddParagraph(GetValue(item, "itemNumber"));
                paragraph = row1.Cells[1].AddParagraph();
                var formattedText = new FormattedText() { Style = "Title" };
                formattedText.AddText(GetValue(item, "title"));
                paragraph.Add(formattedText);
                paragraph.AddFormattedText(" by ", TextFormat.Italic);
                paragraph.AddText(GetValue(item, "author"));
                row2.Cells[1].AddParagraph(GetValue(item, "quantity"));
                row2.Cells[2].AddParagraph(price.ToString("0.00") + " €");
                if (discount > 0)
                    row2.Cells[3].AddParagraph(discount.ToString("0") + '%');
                row2.Cells[4].AddParagraph();
                row2.Cells[5].AddParagraph(price.ToString("0.00"));
                var extendedPrice = quantity * price;
                extendedPrice = extendedPrice * (100 - discount) / 100;
                row1.Cells[5].AddParagraph(extendedPrice.ToString("0.00") + " €");
                row1.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
                totalExtendedPrice += extendedPrice;

                _table.SetEdge(0, _table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
            }*/

            // Add an invisible row as a space line to the table.
            var row = _table.AddRow();
            row.Borders.Visible = false;

            // Add the total price row.
            /*row = _table.AddRow();
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Total Price");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");
            row.Cells[5].Format.Font.Name = "Segoe UI";

            // Add the VAT row.
            row = _table.AddRow();
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("VAT (" + (vat * 100) + "%)");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph((vat * totalExtendedPrice).ToString("0.00") + " €");

            // Add the additional fee row.
            row = _table.AddRow();
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Shipping and Handling");
            row.Cells[5].AddParagraph(0.ToString("0.00") + " €");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;

            // Add the total due row.
            row = _table.AddRow();
            row.Cells[0].AddParagraph("Total Due");
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            totalExtendedPrice += vat * totalExtendedPrice;
            row.Cells[5].AddParagraph(totalExtendedPrice.ToString("0.00") + " €");
            row.Cells[5].Format.Font.Name = "Segoe UI";
            row.Cells[5].Format.Font.Bold = true;

            // Set the borders of the specified cell range.
            _table.SetEdge(5, _table.Rows.Count - 4, 1, 4, Edge.Box, BorderStyle.Single, 0.75);*/

            // Add the notes paragraph.
            if (!string.IsNullOrEmpty(batchModel.Remarks))
            {
                paragraph = _document.LastSection.AddParagraph();
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                paragraph.Format.SpaceBefore = "1cm";
                //paragraph.Format.Borders.Width = 0.75;
                //paragraph.Format.Borders.Distance = 3;
                //paragraph.Format.Borders.Color = TableBorder;
                //paragraph.Format.Shading.Color = TableGray;
                paragraph.AddFormattedText("Remarks: " + batchModel.Remarks);
            }            
        }

        // Some pre-defined colors...
#if true
        // ... in RGB.
        readonly static Color TableBorder = new Color(81, 125, 192);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);
#else
        // ... in CMYK.
        readonly static Color TableBorder = Color.FromCmyk(100, 50, 0, 30);
        readonly static Color TableBlue = Color.FromCmyk(0, 80, 50, 30);
        readonly static Color TableGray = Color.FromCmyk(30, 0, 0, 0, 100);
#endif
    }
}
