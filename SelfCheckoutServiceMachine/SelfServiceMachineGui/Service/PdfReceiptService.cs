using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SelfCheckoutServiceMachine.Service;

public class PdfReceiptService
{
    public void GeneratePdfReceipt(string receiptContent, string filePath)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    
        float receiptWidth = 226.77f;
        float lineHeight = 14f;
        float padding = 30f; 
    
        string[] lines = receiptContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        int numberOfProducts = lines.Length;
        float receiptHeight = (numberOfProducts * lineHeight) + padding;
        
        var pageSize = new iTextSharp.text.Rectangle(receiptWidth, receiptHeight);
    
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            var document = new iTextSharp.text.Document(pageSize, 10, 10, 10, 10);
            PdfWriter.GetInstance(document, fs);
        
            document.Open();
        
            BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(baseFont, 12);
        
            foreach (string line in lines)
            {
                document.Add(new Paragraph(line, font));
            }
        
            document.Close();
        }
    }
    
}