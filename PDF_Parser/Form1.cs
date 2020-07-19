using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDF_Parser
{
    public partial class Form1 : Form
    {
        List<string> s_list = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        public void ExtractTextFromPdf(string pdfFile)
        {
            StringBuilder result = new StringBuilder();
            using (Stream newpdfStream = new FileStream(pdfFile, FileMode.Open, FileAccess.Read))
            {
                PdfReader pdfReader = new PdfReader(newpdfStream);

                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    textBox1.Text = (PdfTextExtractor.GetTextFromPage(pdfReader, i, new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy()));
                }
            }
        }

        public string ShowFileOpenDialog()
        {
            //파일오픈창 생성 및 설정
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF (*.pdf)|*.pdf|모든 파일 (*.*)|*.*";

            //파일 오픈창 로드
            DialogResult dr = ofd.ShowDialog();

            //OK버튼 클릭시
            if (dr == DialogResult.OK)
            {
                //File명과 확장자를 가지고 온다.
                string fileName = ofd.SafeFileName;
                //File경로와 File명을 모두 가지고 온다.
                string fileFullName = ofd.FileName;
                //File경로만 가지고 온다.
                string filePath = fileFullName.Replace(fileName, "");

                //File경로 + 파일명 리턴
                return fileFullName;
            }
            //취소버튼 클릭시 또는 ESC키로 파일창을 종료 했을경우
            else if (dr == DialogResult.Cancel)
            {
                return "";
            }

            return "";
        }

        public void Check_Social_Security_Number(string text)
        {

            string anc = @"[0-9]{2}(0[1-9]|1[012])(0[1-9]|1[0-9]|2[0-9]|3[01])-?[012349][0-9]{6}";
            Regex re = new Regex(anc, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            for (Match m = re.Match(text); m.Success; m = m.NextMatch())
            {
                string result = m.Value.Trim();
                s_list.Add((listView1.Items.Count + 1).ToString());
                s_list.Add("주민번호");
                s_list.Add(result);
                ListViewItem lvl = new ListViewItem(s_list.ToArray());
                listView1.Items.Add(lvl);
                s_list.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = ShowFileOpenDialog();
            ExtractTextFromPdf(link);
            Check_Social_Security_Number(textBox1.Text);
        }
    }
}
