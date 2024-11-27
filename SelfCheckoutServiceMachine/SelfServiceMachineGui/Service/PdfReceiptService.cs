using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = System.Reflection.Metadata.Document;

namespace SelfCheckoutServiceMachine.Service;

public class PdfReceiptService
{
    public void GeneratePdfReceipt(string receiptContent, string filePath)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            var document = new iTextSharp.text.Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, fs);

            document.Open();

            BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(baseFont, 12);

            string[] lines = receiptContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                document.Add(new Paragraph(line, font));
            }

            document.Close();
        }
    }
    
}