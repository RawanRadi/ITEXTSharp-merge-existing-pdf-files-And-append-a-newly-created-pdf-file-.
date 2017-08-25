using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
public partial class forget_Password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void PDF() {
        Response.Clear();
        //here put all your pdf files
        byte[] firstpdffile = System.IO.File.ReadAllBytes("path to first pdf file.pdf");
        //put all bytes of first pdf file in memory stream
        MemoryStream ms = new MemoryStream(firstpdffile);
        //create array of all memory streams to merge between them and add new pdf to the merged pdf
        MemoryStream[] streams = { CreateDoc("name of your new pdf document"), ms };
        //concat all memory streams with the new pdf document
        byte[] bytes = concatmemory(streams);
        Response.ContentType = "application/pdf";
        //inline to open pdf not download it.
        Response.AddHeader("content-disposition", "inline;filename=" + "ss" + ".pdf");
        Response.Buffer = true;
        //put all bytes in one memory stream to display it on the browser 
        MemoryStream stream = new MemoryStream(bytes);
        stream.WriteTo(Response.OutputStream);
        Response.End();
    }
    public static byte[] concatmemory(MemoryStream[] streams)
    {
        //I don't have a web server handy so I'm going to write my final MemoryStream to a byte array and then to disk
        byte[] bytes;
        //Create our final combined MemoryStream
        using (MemoryStream finalStream = new MemoryStream())
        {
            //Create our copy object
            PdfCopyFields copy = new PdfCopyFields(finalStream);

            //Loop through each MemoryStream
            foreach (MemoryStream memory in streams)
            {
                //Reset the position back to zero
                memory.Position = 0;
                //Add it to the copy object
                copy.AddDocument(new PdfReader(memory));
                //Clean up
                memory.Dispose();
            }
            //Close the copy object
            copy.Close();
            //Get the raw bytes to save to disk
            bytes = finalStream.ToArray();
        }
        return bytes;
    }
    private MemoryStream CreateDoc(string name)
    {
        MemoryStream ms = new MemoryStream();
        using (Document doc = new Document(PageSize.LETTER))
        {
            using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
            {
                writer.CloseStream = false;
                doc.Open();
                doc.Add(new Paragraph("you are good programmer"));
                doc.Close();
            }
        }
        return ms;
    }
}
