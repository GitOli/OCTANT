using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Framework;
using IdentitySample.Models;
using Interview;
using Octant.Migrations;
using Syncfusion.CompoundFile.DocIO.Native;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.OfficeChart;
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.HtmlToPdf;
using Syncfusion.Pdf.Parsing;
using ImageType = Syncfusion.HtmlConverter.ImageType;
using Interview = Interview.Interview;

namespace Octant.Controllers
{
    public class GenerateController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Generate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Word(int id, int type, byte[] image,bool pdf = false)
        {
            var dba = new ApplicationDbContext();
            int idCustomer;
            string idConsultant;
            global::Interview.Interview interview = null;
            Campaign campaign = null;

            byte[] byteArrayImg;
            if (type == 1 || type == 2)
            {
                idCustomer = db.Interviews.Where(i => i.IdCampaign == id).Include(i => i.Campaign.Customer).Select(i => i.Campaign.Customer.IdCustomer).FirstOrDefault();
                idConsultant = db.Interviews.Where(i => i.IdCampaign == id).Select(u => u.Id).FirstOrDefault();
                var idFirm = db.Users.Where(u => u.Id == idConsultant).Select(u => u.IdFirm).FirstOrDefault();
                byteArrayImg = ImageController.GetImage("Firms", idFirm.ToString(), true);
            }
            else
            {
                idCustomer = db.Interviews.Where(i => i.IdInterview == id).Include(i => i.Campaign.Customer).Select(i => i.Campaign.Customer.IdCustomer).FirstOrDefault();
                idConsultant = db.Interviews.Where(i => i.IdInterview == id).Select(u => u.Id).FirstOrDefault();
                byteArrayImg = ImageController.GetImage("Users", idConsultant, true);
            }

            var byteArrayCust = ImageController.GetImage("Customers", idCustomer.ToString(), true);
            Stream streamCust = new MemoryStream(byteArrayCust);
            Stream streamImg = new MemoryStream(byteArrayImg);

            WordDocument document = new WordDocument();
            IWSection section = document.AddSection();
            section.PageSetup.DifferentFirstPage = true;
            section.PageSetup.InsertPageNumbers(false, PageNumberAlignment.Center);
            section.PageSetup.Margins.Left = 50;
            section.PageSetup.Margins.Right = 50;

            // Add a new paragraph for header to the document.
            IWParagraph headerPar = new WParagraph(document);

            // Add a new table to the header.
            IWTable table = section.HeadersFooters.FirstPageHeader.AddTable();

            // Styles
            const string font = "Segoe UI";

            Style styleTitle = (WParagraphStyle)document.AddParagraphStyle("Title");
            styleTitle.CharacterFormat.FontName = font;
            styleTitle.CharacterFormat.FontSize = 25;
            styleTitle.CharacterFormat.TextColor = Color.FromArgb(31, 73, 125);

            Style stylePNode = (WParagraphStyle)document.AddParagraphStyle("PNode");
            stylePNode.CharacterFormat.FontName = font;
            stylePNode.CharacterFormat.TextColor = Color.FromArgb(238, 131, 0);
            stylePNode.CharacterFormat.FontSize = 20;

            Style styleNode = (WParagraphStyle)document.AddParagraphStyle("Node");
            styleNode.CharacterFormat.FontName = font;
            styleNode.CharacterFormat.TextColor = Color.FromArgb(195, 23, 0);
            styleNode.CharacterFormat.FontSize = 17;

            Style styleQuestion = (WParagraphStyle)document.AddParagraphStyle("Question");
            styleQuestion.CharacterFormat.FontName = font;
            styleQuestion.CharacterFormat.Bold = true;
            styleQuestion.CharacterFormat.FontSize = 11;

            Style styleAnswer = (WParagraphStyle)document.AddParagraphStyle("Answer");
            styleAnswer.CharacterFormat.FontName = font;
            styleAnswer.CharacterFormat.FontSize = 11;
            styleAnswer.CharacterFormat.TextColor = Color.FromArgb(31, 73, 125);

            Style styleList = (WParagraphStyle)document.AddParagraphStyle("List");
            styleList.CharacterFormat.FontName = font;
            styleList.CharacterFormat.TextColor = Color.FromArgb(51, 132, 204);
            styleList.CharacterFormat.FontSize = 11;
            styleList.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;

            Style styleComment = (WParagraphStyle)document.AddParagraphStyle("Comment");
            styleComment.CharacterFormat.FontName = font;
            styleComment.CharacterFormat.Italic = true;
            styleComment.CharacterFormat.FontSize = 11;

            var format = new RowFormat();

            // Setting cleared table border style.
            format.Borders.BorderType = BorderStyle.Cleared;

            // Inserting table with a row and two columns.
            table.ResetCells(1, 2, format, 265);
            IWTextRange textRange;

            // Inserting logo image to the table cells.
            headerPar = table[0, 0].AddParagraph() as WParagraph;
            headerPar.AppendPicture(Image.FromStream(streamImg));
            (headerPar.Items[0] as WPicture).Width = 50;
            (headerPar.Items[0] as WPicture).Height = 50;
            headerPar.AppendText(Environment.NewLine);
            var userName = db.Users.Find(idConsultant).FullName;

            switch (type)
            {
                case 1:
                case 2:
                    textRange = headerPar.AppendText("Manager: " + userName);
                    textRange.CharacterFormat.FontName = font;
                    headerPar.AppendText(Environment.NewLine);
                    textRange = headerPar.AppendText("Type : Campaign");
                    textRange.CharacterFormat.FontName = font;
                    break;
                case 3:
                case 4:
                    textRange = headerPar.AppendText("Consultant: " + userName);
                    textRange.CharacterFormat.FontName = font;
                    headerPar.AppendText(Environment.NewLine);
                    textRange = headerPar.AppendText("Type: Interview");
                    textRange.CharacterFormat.FontName = font;
                    break;
            }

            headerPar.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
            
            var customerName = db.Customers.Find(idCustomer).Name;
            var industryName =
                db.Customers.Where(c => c.IdCustomer == idCustomer)
                    .Include(c => c.Industry)
                    .Select(c => c.Industry.Name).FirstOrDefault();
            // Inserting text to the table second cell.
            headerPar = table[0, 1].AddParagraph() as WParagraph;
            
            headerPar.AppendPicture(Image.FromStream(streamCust));
            ((WPicture) headerPar.Items[0]).Width = 50;
            ((WPicture) headerPar.Items[0]).Height = 50;
            headerPar.AppendText(Environment.NewLine);
            headerPar.AppendText("Customer: " + customerName);
            headerPar.AppendText(Environment.NewLine);
            headerPar.AppendText("Industry: " + industryName);
            headerPar.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;

            section = document.AddSection();
            section.BreakCode = SectionBreakCode.NoBreak;
            IWParagraph paragraph = section.AddParagraph();

            //TableOfContent toc = paragraph.AppendTOC(1, 3);
            //toc.UseHeadingStyles = false;
            //toc.SetTOCLevelStyle(1, "Title");
            //toc.SetTOCLevelStyle(2, "PNode");
            //toc.SetTOCLevelStyle(3, "Node");

            if (type == 1 || type == 2)
            {
                campaign = db.Campaigns.Find(id);
                var idQuestionnaire = db.Interviews.Where(i => i.Campaign.IdCampaign == id).Include(i => i.Campaign).Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
                var questions = db.Questions.Where(q => q.IdQuestionnaire == idQuestionnaire);
                var questionnairename = db.Questionnaires.Find(idQuestionnaire).Name;
                var interviewees =
                    db.CandidateCampaigns.Include(cc => cc.Candidate).Include(x => x.Group)
                        .Where(cc => cc.IdCampaign == campaign.IdCampaign)
                        .Select(x => x.Candidate);
                var consultants =
                    db.Campaigns.Where(c => c.IdCampaign == campaign.IdCampaign)
                        .Include(c => c.ApplicationUsers)
                        .Select(c => c.ApplicationUsers);
                string start = "";

                if (campaign.StartDate != null)
                {
                    start = campaign.StartDate.Value.ToShortDateString();
                }

                var end = "";

                if (campaign.EndDate != null)
                {
                    end = campaign.EndDate.Value.ToShortDateString();
                }

                paragraph.AppendText(Environment.NewLine + Environment.NewLine + Environment.NewLine);
                textRange = paragraph.AppendText("Campaign: " + campaign.Name);
                paragraph.ApplyStyle("Title");
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);
                paragraph = section.AddParagraph();
                textRange = paragraph.AppendText("Start: " + start + " -> Expected End: " + end);
                textRange.CharacterFormat.FontName = font;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);
                textRange = paragraph.AppendText("Questionnaire: " + questionnairename);
                textRange.CharacterFormat.FontName = font;
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);
                textRange = paragraph.AppendText("Description: " + campaign.Description);
                textRange.CharacterFormat.FontName = font;
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);

                if (type == 2 && !string.IsNullOrEmpty(campaign.Comment))
                {
                    textRange = paragraph.AppendText("Comment: " + campaign.Comment);
                    textRange.CharacterFormat.FontName = font;
                }
                
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);

                paragraph = section.AddParagraph();
                paragraph.AppendText("Consultants: ");
                paragraph.ApplyStyle("List");

                foreach (var consultant in consultants)
                {
                    paragraph = section.AddParagraph();
                    textRange = paragraph.AppendText(consultant.FullName);
                    textRange.CharacterFormat.FontName = font;
                    paragraph.ListFormat.ApplyDefBulletStyle();
                }
                var dbg = new ApplicationDbContext();
                if (type == 2)
                {
                    paragraph = section.AddParagraph();
                    paragraph.AppendText("Interviewees: ");
                    paragraph.ApplyStyle("List");

                    foreach (var interviewee in interviewees)
                    {
                        paragraph = section.AddParagraph();
                        var group = dbg.CandidateCampaigns.Where(cc => cc.IdCandidate == interviewee.IdCandidate).Select(cc => cc.Group).FirstOrDefault();
                        var groupname = "";
                        if (group != null)
                        {
                            groupname = group.Name;
                        }
                        textRange = paragraph.AppendText(interviewee.FullNameAndFunction + " - " + groupname);
                        textRange.CharacterFormat.FontName = font;
                        paragraph.ListFormat.ApplyDefBulletStyle();
                    }
                }

                // Nodes
                var fullnodes = db.Nodes.Where(x => x.IdQuestionnaire == idQuestionnaire).ToList();

                if (fullnodes.Any())
                {
                    var root = fullnodes.FirstOrDefault(r => r.IsRoot);
                    if (root != null)
                    {
                        var nodes = fullnodes.Where(n => !n.IsRoot && n.IdParentNode != root.IdNode).ToList();
                        var pnodes = fullnodes.FindAll(p => p.IdParentNode == root.IdNode).ToList(); 

                        foreach (var pnode in pnodes)
                        {
                            section = document.AddSection();
                            paragraph = section.AddParagraph();
                            paragraph.AppendText(Environment.NewLine);
                            paragraph.AppendText(pnode.Name);
                            paragraph.ApplyStyle("PNode");

                            if (db.Questions.Any(x => x.IdNode == pnode.IdNode))
                            {
                                foreach (var question in questions.Where(q => q.IdNode == pnode.IdNode))
                                {
                                    paragraph = section.AddParagraph();
                                    paragraph.AppendText(question.Description);
                                    paragraph.ApplyStyle("Question");
                                    paragraph.AppendText(Environment.NewLine);

                                    if (!question.IsRelevant)
                                    {
                                        textRange.CharacterFormat.TextColor = Color.DimGray;
                                    }

                                    section.AddParagraph().AppendText(Environment.NewLine);
                                    var dbi = new ApplicationDbContext();

                                    foreach (var answer in dba.Answers.Where(a => a.IdQuestion == question.IdQuestion && a.Interview.IdCampaign == campaign.IdCampaign && !a.Interview.Deleted))
                                    {
                                        var intervname = dbi.Interviews.Find(answer.IdInterview).Name;
                                        paragraph = section.AddParagraph();
                                        paragraph.AppendText("Answer from Interview " + intervname + ":");
                                        paragraph.ApplyStyle("Answer");

                                        if (answer != null && answer.IntervieweeAnswer != null)
                                        {
                                            if (question.AnswerType == AnswerType.Integer)
                                            {
                                                textRange = paragraph.AppendText(answer.IntervieweeAnswer + " / 5");
                                            }
                                            else
                                            {
                                                textRange = paragraph.AppendText(answer.IntervieweeAnswer);
                                            }
                                            textRange.CharacterFormat.FontName = font;
                                            textRange.CharacterFormat.Bold = true;
                                            paragraph.AppendText(Environment.NewLine);
                                        }
                                        else
                                        {
                                            paragraph = section.AddParagraph();
                                            textRange = paragraph.AppendText("No answer yet");
                                            textRange.CharacterFormat.FontName = font;
                                            textRange.CharacterFormat.TextColor = Color.DarkGray;
                                            paragraph.AppendText(Environment.NewLine);
                                        }

                                        if (type == 2 && !string.IsNullOrEmpty(answer.ConsultantComment))
                                        {
                                            if (answer.Comment != null)
                                            {
                                                paragraph = section.AddParagraph();
                                                paragraph.AppendText("Comment: " + answer.Comment);
                                                paragraph.ApplyStyle("Comment");
                                                paragraph.AppendText(Environment.NewLine);
                                            }
                                            paragraph = section.AddParagraph();
                                            paragraph.AppendText("Consultant's comment: " + answer.ConsultantComment);
                                            paragraph.ApplyStyle("Comment");
                                            paragraph.AppendText(Environment.NewLine);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var node in nodes.Where(n => n.IdParentNode == pnode.IdNode))
                                {
                                    paragraph = section.AddParagraph();
                                    paragraph.AppendText(Environment.NewLine);
                                    paragraph.AppendText(" > " + node.Name);
                                    paragraph.ApplyStyle("Node");
                                    paragraph.AppendText(Environment.NewLine);

                                    foreach (var question in questions.Where(q => q.IdNode == node.IdNode))
                                    {
                                        paragraph = section.AddParagraph();
                                        paragraph.AppendText(question.Description);
                                        paragraph.ApplyStyle("Question");
                                        paragraph.AppendText(Environment.NewLine);

                                        if (!question.IsRelevant)
                                        {
                                            textRange.CharacterFormat.TextColor = Color.DimGray;
                                        }
                                        var dbi = new ApplicationDbContext();

                                        foreach (var answer in dba.Answers.Where(a => a.IdQuestion == question.IdQuestion && a.Interview.IdCampaign == campaign.IdCampaign && !a.Interview.Deleted))
                                        {
                                            var intervname = dbi.Interviews.Find(answer.IdInterview).Name;
                                            paragraph = section.AddParagraph();
                                            paragraph.AppendText("Answer from Interview " + intervname + ":");
                                            paragraph.ApplyStyle("Answer");

                                            if (answer.IntervieweeAnswer != null)
                                            {
                                                paragraph = section.AddParagraph();
                                                if (question.AnswerType == AnswerType.Integer)
                                                {
                                                    textRange = paragraph.AppendText(answer.IntervieweeAnswer + " / 5");
                                                }
                                                else
                                                {
                                                    textRange = paragraph.AppendText(answer.IntervieweeAnswer);
                                                }
                                                textRange.CharacterFormat.FontName = font;
                                                textRange.CharacterFormat.Bold = true;
                                                paragraph.AppendText(Environment.NewLine);
                                            }
                                            else
                                            {
                                                paragraph = section.AddParagraph();
                                                textRange = paragraph.AppendText("No answer yet");
                                                textRange.CharacterFormat.FontName = font;
                                                textRange.CharacterFormat.TextColor = Color.DarkGray;
                                                paragraph.AppendText(Environment.NewLine);
                                            }

                                            if (type == 2 && !string.IsNullOrEmpty(answer.ConsultantComment))
                                            {
                                                if (answer.Comment != null)
                                                {
                                                    paragraph = section.AddParagraph();
                                                    paragraph.AppendText("Comment: " + answer.Comment);
                                                    paragraph.ApplyStyle("Comment");
                                                    paragraph.AppendText(Environment.NewLine);
                                                }
                                                paragraph = section.AddParagraph();
                                                paragraph.AppendText("Consultant's comment: " + answer.ConsultantComment);
                                                paragraph.ApplyStyle("Comment");
                                                paragraph.AppendText(Environment.NewLine);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (type == 3 || type == 4)
            {
                interview = db.Interviews.Find(id);
                var idInterviewee = db.Interviewees.Where(i => i.IdInterview == id).Select(i => i.IdCandidate).FirstOrDefault();
                var idQuestionnaire = db.Interviews.Where(i => i.IdInterview == id).Include(i => i.Campaign).Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
                var questions = db.Questions.Where(q => q.IdQuestionnaire == idQuestionnaire);
                var interviewees =
                    db.Interviewees.Include(cc => cc.Candidate)
                        .Where(cc => cc.IdInterview == interview.IdInterview)
                        .Select(x => x.Candidate);
                var consultant = interview.ApplicationUsers;

                paragraph.AppendText(Environment.NewLine + Environment.NewLine + Environment.NewLine);
                paragraph.AppendText("Interview: " + interview.Name);
                paragraph.ApplyStyle("Title");
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);
                paragraph = section.AddParagraph();
                textRange = paragraph.AppendText("Date: " + interview.Date.ToShortDateString() + Environment.NewLine);
                textRange.CharacterFormat.FontName = font;
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);

                textRange = paragraph.AppendText("Description: " + interview.Description);
                textRange.CharacterFormat.FontName = font;
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);

                if (type == 4 && interview.Comment != null)
                {
                    textRange = paragraph.AppendText("Comment: " + interview.Comment + Environment.NewLine);
                    textRange.CharacterFormat.FontName = font;
                }

                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                paragraph.AppendText(Environment.NewLine + Environment.NewLine);

                paragraph = section.AddParagraph();
                paragraph.AppendText("Consultants: ");
                paragraph.ApplyStyle("List");
                
                    paragraph = section.AddParagraph();
                    textRange = paragraph.AppendText(consultant.FullName);
                    textRange.CharacterFormat.FontName = font;
                    paragraph.ListFormat.ApplyDefBulletStyle();
                
                paragraph = section.AddParagraph();
                paragraph.AppendText("Interviewee(s): ");
                paragraph.ApplyStyle("List");

                var dbc = new ApplicationDbContext();

                foreach (var i in interviewees)
                {
                    paragraph = section.AddParagraph();
                    var group =
                        dbc.CandidateCampaigns.Where(
                            cc => cc.IdCandidate == i.IdCandidate && cc.IdCampaign == interview.IdCampaign).Select(cc => cc.Group).FirstOrDefault();
                    var groupname = "";
                    if (group != null)
                    {
                        groupname = group.Name;
                    }
                    textRange = paragraph.AppendText(i.FullNameAndFunction + " - " + groupname);
                    textRange.CharacterFormat.FontName = font;
                    textRange.CharacterFormat.Bold = false;
                    paragraph.ListFormat.ApplyDefBulletStyle();
                }

                var fullnodes = db.Nodes.Where(x => x.IdQuestionnaire == idQuestionnaire).ToList();
                
                if (fullnodes.Any())
                {
                    var root = fullnodes.FirstOrDefault(r => r.IsRoot);
                    if (root != null)
                    {
                        var nodes = fullnodes.Where(n => !n.IsRoot && n.IdParentNode != root.IdNode).ToList();
                        var pnodes = fullnodes.FindAll(p => p.IdParentNode == root.IdNode).ToList();

                        foreach (var pnode in pnodes)
                        {
                            var score = db.CustomScores.FirstOrDefault(c => c.IdNode == pnode.IdNode && c.IdInterview == interview.IdInterview);
                            var val = 0;

                            if (score != null)
                            {
                                val = score.Value != null ? score.Value : 0;
                            }
                            section = document.AddSection();
                            paragraph = section.AddParagraph();
                            paragraph.AppendText(Environment.NewLine);
                            paragraph.AppendText(pnode.Name + "     " + val + " / 5");
                            paragraph.ApplyStyle("PNode");

                            if (db.Questions.Any(x => x.IdNode == pnode.IdNode))
                            {
                                foreach (var question in questions.Where(q => q.IdNode == pnode.IdNode))
                                {
                                    var answer = dba.Answers.FirstOrDefault(a => a.IdQuestion == question.IdQuestion && a.IdInterview == id);

                                    paragraph = section.AddParagraph();
                                    paragraph.AppendText(question.Description);
                                    paragraph.ApplyStyle("Question");
                                    paragraph.AppendText(Environment.NewLine);
                                    if (answer != null && answer.IntervieweeAnswer != null)
                                    {
                                        paragraph = section.AddParagraph();
                                        paragraph.AppendText("Answer:");
                                        paragraph.ApplyStyle("Answer");
                                        paragraph = section.AddParagraph();

                                        if (question.AnswerType == AnswerType.Integer)
                                        {
                                            paragraph.AppendText(answer.IntervieweeAnswer + " / 5");
                                        }
                                        else
                                        {
                                            paragraph.AppendText(answer.IntervieweeAnswer);
                                        }
                                        textRange.CharacterFormat.FontName = font;
                                        textRange.CharacterFormat.Bold = true;
                                        paragraph.AppendText(Environment.NewLine);

                                        if (type == 4 && !string.IsNullOrEmpty(answer.ConsultantComment))
                                        {
                                            if (answer.Comment != null)
                                            {
                                                paragraph = section.AddParagraph();
                                                paragraph.AppendText("Comment: " + answer.Comment);
                                                paragraph.ApplyStyle("Comment");
                                                paragraph.AppendText(Environment.NewLine);
                                            }
                                            paragraph = section.AddParagraph();
                                            paragraph.AppendText("Consultant's comment: " + answer.ConsultantComment);
                                            paragraph.ApplyStyle("Comment");
                                            paragraph.AppendText(Environment.NewLine);
                                        }
                                    }
                                    else
                                    {
                                        paragraph = section.AddParagraph();
                                        textRange = paragraph.AppendText("No answer yet");
                                        textRange.CharacterFormat.FontName = font;
                                        textRange.CharacterFormat.TextColor = Color.DarkGray;
                                        paragraph.AppendText(Environment.NewLine);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var node in nodes.Where(n => n.IdParentNode == pnode.IdNode))
                                {
                                    paragraph = section.AddParagraph();
                                    paragraph.AppendText(Environment.NewLine);
                                    paragraph.AppendText(" > " + node.Name + "     " + val + " / 5");
                                    paragraph.ApplyStyle("Node");
                                    paragraph.AppendText(Environment.NewLine);

                                    foreach (var question in questions.Where(q => q.IdNode == node.IdNode))
                                    {
                                        var answer = dba.Answers.FirstOrDefault(a => a.IdQuestion == question.IdQuestion && a.IdInterview == id);

                                        paragraph = section.AddParagraph();
                                        paragraph.AppendText(question.Description);
                                        paragraph.ApplyStyle("Question");
                                        paragraph.AppendText(Environment.NewLine);

                                        if (answer != null && answer.IntervieweeAnswer != null)
                                        {
                                            paragraph = section.AddParagraph();
                                            paragraph.AppendText("Answer:");
                                            paragraph.ApplyStyle("Answer");
                                            paragraph = section.AddParagraph();
                                            if (question.AnswerType == AnswerType.Integer)
                                            {
                                                paragraph.AppendText(answer.IntervieweeAnswer + " / 5");
                                            }
                                            else
                                            {
                                                paragraph.AppendText(answer.IntervieweeAnswer);
                                            }
                                            textRange.CharacterFormat.FontName = font;
                                            textRange.CharacterFormat.Bold = true;
                                            paragraph.AppendText(Environment.NewLine);

                                            if (type == 4 && !string.IsNullOrEmpty(answer.ConsultantComment))
                                            {
                                                if (answer.Comment != null)
                                                {
                                                    paragraph = section.AddParagraph();
                                                    paragraph.AppendText("Comment: " + answer.Comment);
                                                    paragraph.ApplyStyle("Comment");
                                                    paragraph.AppendText(Environment.NewLine);
                                                }

                                                paragraph = section.AddParagraph();
                                                paragraph.AppendText("Consultant's comment: " + answer.ConsultantComment);
                                                paragraph.ApplyStyle("Comment");
                                                paragraph.AppendText(Environment.NewLine);
                                            }
                                        }
                                        else
                                        {
                                            paragraph = section.AddParagraph();
                                            textRange = paragraph.AppendText("No answer yet");
                                            textRange.CharacterFormat.FontName = font;
                                            textRange.CharacterFormat.TextColor = Color.DarkGray;
                                            paragraph.AppendText(Environment.NewLine);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            switch (type)
            {
                case 1:
                    if (!pdf)
                    {
                        return document.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + campaign.Name + "_Customer.docx", FormatType.Word2013, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                    }
                    else
                    {
                        var converted = ConvertToPDF(document);
                        return converted.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + campaign.Name + "_Customer.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
                    }
                case 2:
                    if (!pdf)
                    {
                        return document.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + campaign.Name + "_Firm.docx", FormatType.Word2013, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                    }
                    else
                    {
                        var converted = ConvertToPDF(document);
                        return converted.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + campaign.Name + "_Firm.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
                    }
                case 3:
                    if (!pdf)
                    {
                        return document.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + interview.Name + "_Customer.docx", FormatType.Word2013, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                    }
                    else
                    {
                        var converted = ConvertToPDF(document);
                        return converted.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + interview.Name + "_Customer.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
                    }
                case 4:
                    if (!pdf)
                    {
                        return document.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + interview.Name + "_Firm.docx", FormatType.Word2013, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
                    }
                    else
                    {
                        var converted = ConvertToPDF(document);
                        return converted.ExportAsActionResult(DateTime.Now.ToShortDateString() + "_" + interview.Name + "_Firm.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
                    }
            }

            return document.ExportAsActionResult("Report.docx", FormatType.Word2013, HttpContext.ApplicationInstance.Response, HttpContentDisposition.Attachment);
        }

        protected PdfDocument ConvertToPDF(WordDocument document)
        {
            DocToPDFConverter converter = new DocToPDFConverter();
            //Convert the Word document into a PDF document.
            PdfDocument pdfDoc = converter.ConvertToPDF(document);
            return pdfDoc;
        }

        protected string ResolveApplicationDataPath(string folderName)
        {
            string dataPath = Request.PhysicalPath.ToLower();
            if (dataPath.EndsWith("\\"))
                dataPath = dataPath.TrimEnd('\\');
            if (folderName != string.Empty)
                dataPath += "\\" + folderName;
            return dataPath;
        }
        protected string ResolveApplicationDataPath(string fileName, string folderName)
        {
            string dataPath = Request.PhysicalPath.ToLower();
            if (dataPath.EndsWith("\\"))
                dataPath = dataPath.TrimEnd('\\');
            if (folderName != string.Empty)
                dataPath += "\\" + folderName;
            return string.Format("{0}\\{1}", dataPath, fileName);
        }
    }
    
    public class DocumentResult : ActionResult
    {
        public string FileName { get; set; }

        public IWordDocument Document { get; private set; }

        public FormatType formatType { get; set; }

        public HttpContentDisposition ContentDisposition { get; set; }

        public HttpResponse Response { get; private set; }

        public DocumentResult(IWordDocument document, string filename, FormatType formattype, HttpResponse response, HttpContentDisposition contentDisposition)
        {
            FileName = filename;
            Document = document;
            formatType = formattype;
            Response = response;
            ContentDisposition = contentDisposition;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context");
            Document.Save(FileName, formatType, Response, ContentDisposition);
        }
    }

    public static class DocIOExtension
    {
        public static DocumentResult ExportAsActionResult(this IWordDocument document, string filename, FormatType formattype, HttpResponse response, HttpContentDisposition contentDisposition)
        {
            return new DocumentResult(document, filename, formattype, response, contentDisposition);
        }
    }

    public static class PdfExtension

    {
        public static PdfResult ExportAsActionResult(this PdfDocument PdfDoc, string filename, HttpResponse response, HttpReadType type)
        {
            return new PdfResult(PdfDoc, filename, response, type);
        }

        public static PdfResult ExportAsActionResult(this PdfLoadedDocument pdfdoc, string filename, HttpResponse response, HttpReadType type)
        {
            return new PdfResult(pdfdoc, filename, response, type);
        }
    }

    public class PdfResult : ActionResult
    {
        private readonly PdfDocument m_pdfDocument;

        private readonly PdfLoadedDocument m_pdfLoadedDocument;

        public string FileName { get; set; }

        public PdfDocument PdfDoc
        {
            get
            {
                if (m_pdfDocument != null)
                    return m_pdfDocument;

                return null;
            }
        }

        public PdfLoadedDocument pdfLoadedDoc
        {
            get
            {
                if (m_pdfLoadedDocument != null)
                    return m_pdfLoadedDocument;

                return null;
            }
        }

        public HttpResponse Response { get; private set; }

        public HttpReadType ReadType { get; set; }

        public PdfResult(PdfDocument pdfDocument, string filename, HttpResponse respone, HttpReadType type)
        {
            m_pdfDocument = pdfDocument;

            m_pdfLoadedDocument = null;

            FileName = filename;

            Response = respone;

            ReadType = type;
        }

        public PdfResult(PdfLoadedDocument pdfLoadedDocument, string filename, HttpResponse respone, HttpReadType type)
        {
            m_pdfDocument = null;

            m_pdfLoadedDocument = pdfLoadedDocument;

            FileName = filename;

            Response = respone;

            ReadType = type;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                return;

            if (pdfLoadedDoc != null)
                pdfLoadedDoc.Save(FileName, Response, ReadType);

            if (PdfDoc != null)
            {
                PdfDoc.Save(FileName, Response, ReadType);

                PdfDoc.Close(true);
            }
        }
    }
}