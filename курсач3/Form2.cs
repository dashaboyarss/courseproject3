using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
//using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ClosedXML.Excel;

namespace курсач3
{
    internal class Form2: Form
    {
        public Form2()
        {
            Text = "Сохраненный план";
            Size = new System.Drawing.Size(1103, 711);
            BackColor = System.Drawing.Color.LightGray;

            AddLabels();
            AddTextBoxes();
            AddPaymentBlock();
        }

        private void AddLabels()
        {
            Label labelName = new Label();
            Label labelGoal = new Label();
            Label labelCurrentSum = new Label();
            Label labelFreq = new Label();
            Label labelInvestPercent = new Label();
            Label labelInvestAmount = new Label();
            Label labelInflation = new Label();
            Label labelGoalWithInflation = new Label();
            Label labelInvestIncome = new Label();
            Label labelPaymentAmount = new Label();
            Label labelTime = new Label();

            int y = 29;
            labelName.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelName.Text = "Название плана:";
            labelName.Size = new System.Drawing.Size(224, 13);

            labelGoal.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelGoal.Text = "Сумма цели:";
            labelGoal.Size = new System.Drawing.Size(224, 13);

            labelFreq.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelFreq.Text = "Частота взносов:";
            labelFreq.Size = new System.Drawing.Size(224, 13);

            labelInflation.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelInflation.Text = "Текущий уровень инфляции (%):";
            labelInflation.Size = new System.Drawing.Size(224, 13);

            labelInvestPercent.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelInvestPercent.Text = "Ожидаемая доходность от инвестиций (%):";
            labelInvestPercent.Size = new System.Drawing.Size(224, 13);

            labelInvestAmount.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelInvestAmount.Text = "Текущая сумма на инвестиционном счету:";
            labelInvestAmount.Size = new System.Drawing.Size(224, 13);

            labelCurrentSum.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelCurrentSum.Text = "Текущие накопления без инвестиций:";
            labelCurrentSum.Size = new System.Drawing.Size(224, 13);

            int y1 = 24;
            labelGoalWithInflation.Text = "Сумма цели с учетом инфляции:";
            labelGoalWithInflation.Font = new System.Drawing.Font(labelGoalWithInflation.Font.FontFamily, 11, FontStyle.Regular);
            labelGoalWithInflation.Location = new Point(335, y1);
            labelGoalWithInflation.Size = new Size(392, 26);
            y1 += 65;

            labelInvestIncome.Text = "Текущий доход от инвестиций:";
            labelInvestIncome.Font = new System.Drawing.Font(labelInvestIncome.Font.FontFamily, 11, FontStyle.Regular);
            labelInvestIncome.Location = new Point(335, y1);
            labelInvestIncome.Size = new Size(392, 26);
            y1 += 65;

            labelPaymentAmount.Text = "Размер взносов:";
            labelPaymentAmount.Font = new System.Drawing.Font(labelPaymentAmount.Font.FontFamily, 11, FontStyle.Regular);
            labelPaymentAmount.Location = new Point(335, y1);
            labelPaymentAmount.Size = new Size(392, 26);
            y1 += 65;

            labelTime.Text = "Срок достижения цели:";
            labelTime.Font = new System.Drawing.Font(labelTime.Font.FontFamily, 11, FontStyle.Regular);
            labelTime.Location = new Point(335, y1);
            labelTime.Size = new Size(392, 26);
            y1 += 65;

            this.Controls.Add(labelName);
            this.Controls.Add(labelGoal);
            this.Controls.Add(labelFreq);
            this.Controls.Add(labelInflation);
            this.Controls.Add(labelInvestPercent);
            this.Controls.Add(labelInvestAmount);
            this.Controls.Add(labelCurrentSum);
            this.Controls.Add(labelGoalWithInflation);
            this.Controls.Add(labelInvestIncome);
            this.Controls.Add(labelPaymentAmount);
            this.Controls.Add(labelTime);
        }

        private void AddTextBoxes()
        {
            TextBox nameTextBox = new TextBox();
            TextBox goalTextBox = new TextBox();
            ComboBox freqComboBox = new ComboBox();
            TextBox inflationTextBox = new TextBox();
            TextBox investPercentTextBox = new TextBox();
            TextBox investAmountTextBox = new TextBox();
            TextBox currentSumTextBox = new TextBox();
            RichTextBox goalWithInflationRichTextBox = new RichTextBox();
            RichTextBox investIncomeRichTextBox = new RichTextBox();
            RichTextBox paymentAmountRichTextBox = new RichTextBox();
            RichTextBox timeRichTextBox = new RichTextBox();

            int y = 45;

            nameTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            nameTextBox.Size = new System.Drawing.Size(169, 20);
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.ReadOnly = true;

            goalTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            goalTextBox.Size = new System.Drawing.Size(131, 20);
            goalTextBox.BorderStyle = BorderStyle.FixedSingle;
            //goalTextBox.ReadOnly = true;

            freqComboBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            freqComboBox.Size = new System.Drawing.Size(131, 20);
            string[] choices = {"раз в неделю", "раз в 2 недели", "раз в месяц", "раз в 3 месяца", "раз в полгода", "раз в год"};
            freqComboBox.Items.AddRange(choices);

            inflationTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            inflationTextBox.Size = new System.Drawing.Size(131, 20);
            inflationTextBox.BorderStyle = BorderStyle.FixedSingle;

            investPercentTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            investPercentTextBox.Size = new System.Drawing.Size(131, 20);
            investPercentTextBox.BorderStyle = BorderStyle.FixedSingle;

            investAmountTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            investAmountTextBox.Size = new System.Drawing.Size(131, 20);
            investAmountTextBox.BorderStyle = BorderStyle.FixedSingle;
            investAmountTextBox.ReadOnly = true;

            currentSumTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            currentSumTextBox.Size = new System.Drawing.Size(131, 20);
            currentSumTextBox.BorderStyle = BorderStyle.FixedSingle;
            currentSumTextBox.ReadOnly = true;

            int y1 = 52;
            goalWithInflationRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            goalWithInflationRichTextBox.Size = new System.Drawing.Size(290, 29);
            goalWithInflationRichTextBox.Font = new Font(goalWithInflationRichTextBox.Font.FontFamily, 11, FontStyle.Regular);
            goalWithInflationRichTextBox.ReadOnly = true;

            investIncomeRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            investIncomeRichTextBox.Size = new System.Drawing.Size(290, 29);
            investIncomeRichTextBox.Font = new Font(investIncomeRichTextBox.Font.FontFamily, 11, FontStyle.Regular);
            investIncomeRichTextBox.ReadOnly= true;

            paymentAmountRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            paymentAmountRichTextBox.Size = new System.Drawing.Size(290, 29);
            paymentAmountRichTextBox.Font = new Font(paymentAmountRichTextBox.Font.FontFamily, 11, FontStyle.Regular);

            timeRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            timeRichTextBox.Size = new System.Drawing.Size(290, 29);
            timeRichTextBox.Font = new Font(timeRichTextBox.Font.FontFamily, 11, FontStyle.Regular);

            this.Controls.Add(nameTextBox);
            this.Controls.Add(goalTextBox);
            this.Controls.Add(freqComboBox);
            this.Controls.Add(inflationTextBox);
            this.Controls.Add(investPercentTextBox);
            this.Controls.Add(investAmountTextBox);
            this.Controls.Add(currentSumTextBox);
            this.Controls.Add(goalWithInflationRichTextBox);
            this.Controls.Add(investIncomeRichTextBox);
            this.Controls.Add(paymentAmountRichTextBox);
            this.Controls.Add(timeRichTextBox);
        }

        private void AddPaymentBlock()
        {
            this.Paint += new PaintEventHandler(DrawRectangle);

            Label label = new Label();
            label.Text = "Введите сумму взноса:";
            label.Location = new System.Drawing.Point(760, 82);
            label.Size = new System.Drawing.Size(227, 26);
            label.Font = new Font(label.Font.FontFamily, 10, FontStyle.Regular);
            label.BackColor = System.Drawing.Color.Transparent;

            this.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(760, 112);
            textBox.Size = new System.Drawing.Size(219, 20);
            textBox.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(textBox);

            Button button = new Button();
            button.Text = "ВНЕСТИ";
            button.Size = new System.Drawing.Size(90, 44);
            button.Location = new System.Drawing.Point(760 + 55, 142); 

            this.Controls.Add(button);
        }

        private void DrawRectangle(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            GraphicsPath path = new GraphicsPath();
            int cornerRadius = 40;
            int x = 750;
            int y = 52;
            int width = 250;
            int height = 150;

            Color intermediateColor = InterpolateColors(Color.LightGray, Color.DimGray, 0.5f);
            path.AddArc(x, y, cornerRadius, cornerRadius, 180, 90); 
            path.AddArc(x + width - cornerRadius, y, cornerRadius, cornerRadius, 270, 90); 
            path.AddArc(x + width - cornerRadius, y + height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(x, y + height - cornerRadius, cornerRadius, cornerRadius, 90, 90); 

            path.CloseFigure();

            using (SolidBrush brush = new SolidBrush(intermediateColor))
            {
                g.FillPath(brush, path); 
            }

            
            using (Pen pen = new Pen(Color.Black, 3)) 
            {
                g.DrawPath(pen, path); 
            }
        }

        private Color InterpolateColors(Color color1, Color color2, float ratio)
        {
            ratio = Math.Max(0, Math.Min(1, ratio));

            int r = (int)(color1.R + (color2.R - color1.R) * ratio);
            int g = (int)(color1.G + (color2.G - color1.G) * ratio);
            int b = (int)(color1.B + (color2.B - color1.B) * ratio);

            return Color.FromArgb(r, g, b);
        }

        private void AddChart()
        {
            Chart chart = new Chart();
            
        }
    }
}
