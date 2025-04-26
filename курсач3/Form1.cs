using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;

namespace курсач3
{
    public partial class Form1 : Form
    {
        private List<Plan> plans = new List<Plan>();
        
        bool isCorrect = true; //флаг, проверяющий корректность всех введенных данных
        public static Payments planPayments;
        public static Period planPeriod;
        int targetSum;
        int startSum;
        double investmentPerCent;
        double investmentSum;
        int targetPeriod;
        double inflation;
        double paymentAmount;

        int countSavedPlan = 0;

        int countPanel = 0;

        public static Form1 CurrentForm { get; private set; }

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            CurrentForm = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillList();

            AddPlanPanels();
        }

        public TabPage GetTabPagePlans() => tabPage1;

        private void FillList()
        {
            var wbook = new XLWorkbook();
            string filePath = "simple.xlsx";
            if (File.Exists(filePath))
            {
                using (wbook = new XLWorkbook(filePath))
                {
                    var ws = wbook.Worksheet(1);

                    int count = 2;
                    while (!ws.Cell($"A{count}").IsEmpty())
                    {
                        string name = ws.Cell($"A{count}").Value.ToString();
                        double goal = double.Parse(ws.Cell($"B{count}").Value.ToString());
                        double startAmount = double.Parse(ws.Cell($"C{count}").Value.ToString());
                        string frequency = ws.Cell($"D{count}").Value.ToString();
                        double investPercent = double.Parse(ws.Cell($"E{count}").Value.ToString());
                        double investAmount = double.Parse(ws.Cell($"F{count}").Value.ToString());
                        int time = int.Parse(ws.Cell($"G{count}").Value.ToString());
                        double inflation = double.Parse(ws.Cell($"H{count}").Value.ToString());
                        double goalWithInflation = double.Parse(ws.Cell($"I{count}").Value.ToString());
                        double investIncome = double.Parse(ws.Cell($"J{count}").Value.ToString());
                        double paymentAmount = double.Parse(ws.Cell($"K{count}").Value.ToString());
                        int paymentCount = int.Parse(ws.Cell($"L{count}").Value.ToString());

                        Plan plan = new Plan(name, goal, startAmount, frequency, investPercent, investAmount, inflation, goalWithInflation, investIncome, time, paymentAmount, paymentCount);
                        plans.Add(plan);
                        countSavedPlan++;

                        count++;
                    }
                }
            }
            
        }

        public void AddPlanPanels()
        {
            foreach (Plan item in plans)
            {
                //AddPanel(item);
                Panel.AddPanel(item);
            }
        }


        static bool ParseInput(string input)
        {
            bool isConverted;
            int result;
            isConverted = Int32.TryParse(input, out result);
            return isConverted;
        }

        bool CorrectInputPeriod()
        {
            bool isCorrect;
            bool result = true;

            isCorrect = ParseInput(goalAmountTextBox2.Text);
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Сумма цели'! Введите целое число больше 0");
            }
            isCorrect = ParseInput(startAmountTextBox2.Text);
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Начальная сумма без инвестиций'! Введите целое неотрицательное число");
            }
            string freq = frequencyComboBox1.Text;
            isCorrect = freq == "раз в неделю" || freq == "раз в 2 недели" || freq == "раз в месяц" || freq == "раз в 3 месяца" || freq == "раз в полгода" || freq == "раз в год";
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Частота взносов'! Выберите один из предложенных вариантов");
            }
            isCorrect = ParseInput(investIncomeTextBox2.Text);
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Ожидаемая доходность от инвестииций'! Введите целое число от 0 до 100");
            }
            isCorrect = ParseInput(investAmountTextBox2.Text);
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Сумма на инвестиционном счету'! Введите целое неотрицательное число");
            }
            isCorrect = ParseInput(paymentTextBox.Text);
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Размер взносов'! Введите целое положмтельное число");
            }
            isCorrect = ParseInput(inflationTextBox2.Text);
            if (!isCorrect)
            {
                result = false;
                MessageBox.Show("Неверно введено значения поля 'Текущий уровень инфляции'! Введите целое неотрицательное число");
            }
            return result;
        }

        bool CorrectInputPayments()
        {
            bool isConverted;
            bool result = true;

            isConverted = ParseInput(goalAmountTextBox.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля 'Сумма цели!'");
                result = false;
            }
            isConverted = ParseInput(startSumTextBox.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля 'Начальная сумма!'");
                result = false;
            }
            isConverted = ParseInput(incomePercentTextBox.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля 'Ожмдаемый доход от инвестиций'!");
                result = false;
            }
            isConverted = ParseInput(investAmountTextBox.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Сумма на инвестиционном счету!");
                result = false;
            }
            isConverted = ParseInput(timeTextBox.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Желаемый срок достижения цели!");
                result = false;
            }
            isConverted = ParseInput(inflationTextBox.Text);
            if (!isConverted)
            {
                MessageBox.Show("Некорректно введено значение поля Текущий уровень инфляции (%)!");
                result = false;
            }
            if (frequencyComboBox.Text != "раз в неделю" && frequencyComboBox.Text != "раз в 2 недели" && frequencyComboBox.Text != "раз в месяц" && frequencyComboBox.Text != "раз в 3 месяца" && frequencyComboBox.Text != "раз в полгода" && frequencyComboBox.Text != "раз в год")
            {
                MessageBox.Show("Некорректно заполнено поле частоты взносов! Выберите один из предложенных вариантов внесения взносов");
                result = false;
            }

            return result;
        }
        static double Step(string freq)
        {
            double step = 0;
            if (freq == "раз в неделю")
            {
                step = 0.25;
            }
            else if (freq == "раз в 2 недели")
            {
                step = 0.5;
            }
            else if (freq == "раз в месяц")
            {
                step = 1;
            }
            else if (freq == "раз в 3 месяца")
            {
                step = 3;
            }
            else if (freq == "раз в полгода")
            {
                step = 6;
            }
            else if (freq == "раз в год")
            {
                step = 12;
            }
            return step;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (CorrectInputPayments())
            {
                targetSum = Convert.ToInt32(goalAmountTextBox.Text); //целевая сумма
                startSum = Convert.ToInt32(this.startSumTextBox.Text);//начальная сумма
                investmentPerCent = Convert.ToDouble(incomePercentTextBox.Text) / 100; //процент дохода от инвестиций
                investmentSum = Convert.ToDouble(investAmountTextBox.Text); //сумма в инвестициях
                targetPeriod = Convert.ToInt32(timeTextBox.Text); //целевой срок наклопления
                inflation = Convert.ToDouble(inflationTextBox.Text) / 100; //годовой процент инфляции
                planPayments = new Payments(nameTextBox.Text, targetSum, startSum, frequencyComboBox.Text, investmentPerCent, investmentSum, targetPeriod, inflation);
                if (planPayments.isCorrect)
                {
                    bool checkSum = planPayments.CheckSum();
                    bool checkTime = planPayments.CheckTime();
                    //bool checkProgress = planPayments.CheckDifference();
                    if (checkSum && checkTime)
                    {
                        planPayments.NewGoalAmount();
                        planPayments.InvestIncome();
                        planPayments.PaymentAmount();
                        planPayments.CountPayments();

                        newGoalAmountRichTextBox.Text = planPayments.amountWithInflation.ToString();
                        investIncomeRichTextBox.Text = planPayments.investIncome.ToString();
                        paymentRichTextBox.Text = planPayments.paymentAmount.ToString();
                        countPaymentsRichTextBox1.Text = planPayments.countPayments.ToString();

                        double payment = planPayments.paymentAmount;
                        double endGraph = targetPeriod;
                        double step = Step(frequencyComboBox.Text);
                        double x = 0;
                        double y = startSum + investmentSum;
                        this.chart.Series[0].Points.Clear();
                        this.chart.Series[1].Points.Clear();
                        while (x <= endGraph)
                        {
                            this.chart.Series[0].Points.AddXY(x, y);
                            x += step;
                            y += payment;
                        }

                        double endGraph1 = targetPeriod;
                        double x1 = 0;
                        double step1 = 1;
                        double y1 = targetSum;
                        while (x1 <= endGraph1)
                        {
                            this.chart.Series[1].Points.AddXY(x1, y1);
                            x1 += step1;
                            y1 += y1 * (inflation / 12);
                        }
                    }
                }
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool isConverted = CorrectInputPeriod();
            if (isConverted)
            {
                targetSum = Convert.ToInt32(goalAmountTextBox2.Text);
                startSum = Convert.ToInt32(startAmountTextBox2.Text);
                investmentPerCent = Convert.ToDouble(investIncomeTextBox2.Text) / 100;
                investmentSum = Convert.ToDouble(investAmountTextBox2.Text);
                paymentAmount = Convert.ToDouble(paymentTextBox.Text);
                inflation = Convert.ToDouble(inflationTextBox2.Text) / 100;

                planPeriod = new Period(nameTextBox2.Text, targetSum, startSum, frequencyComboBox1.Text, investmentPerCent, investmentSum, paymentAmount, inflation);

                if (planPeriod.isCorrect)
                {
                    bool checkSum = planPeriod.CheckSum();
                    //bool checkTime = planPeriod.CheckTime();
                    bool checkProgress = planPeriod.CheckDifference();

                    if (checkSum /*&& checkTime*/ && checkProgress)
                    {
                        planPeriod.CountTime();
                        planPeriod.InvestIncome();
                        planPeriod.NewGoalAmount();
                        planPeriod.CountPayments();

                        newGoalAmountWithInflationRichTextBox.Text = planPeriod.amountWithInflation.ToString();
                        investIncomeRichTextBox2.Text = planPeriod.investIncome.ToString();
                        timeRichTextBox2.Text = planPeriod.TimeToYears();
                        countPaymentsRichTextBox2.Text = planPeriod.countPayments.ToString();

                        int b = planPeriod.time / 4;
                        double x = 0;
                        double y = startSum + investmentSum;

                        double step = Step(frequencyComboBox1.Text);
                        this.chart1.Series[1].Points.Clear();
                        this.chart1.Series[0].Points.Clear();
                        while (x <= b)
                        {
                            this.chart1.Series[1].Points.AddXY(x, y);
                            x += step;
                            y += planPeriod.paymentAmount;
                        }

                        int b1 = planPeriod.time / 4;
                        double x1 = 0;
                        double y1 = planPeriod.GoalAmount;
                        double step1 = 1;
                        while (x1 <= b1)
                        {
                            this.chart1.Series[0].Points.AddXY(x1, y1);
                            x1 += step1;
                            y1 += y1 * (planPeriod.inflation / 12);
                        }
                    }

                }
            }


        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_Period(object sender, EventArgs e)
        {
            plans.Add(planPeriod);

            SavePlanPeriod();

            Panel.AddPanel(planPeriod);

            ClearAllTextBoxesInTabPage(CurrentForm);
            ClearChart1();
        }

        public void ClearChart1()
        {     
            chart1.Annotations.Clear();
            foreach (var series in chart1.Series)
            {
                series.Points.Clear(); // Очищает точки данных, но оставляет серию
            }
        }

        public void ClearAllTextBoxesInTabPage(Form1 form)
        {
            foreach (System.Windows.Forms.Control control in form.Controls)
            {
                ClearTextBoxesRecursive(control);
            }
        }

        private void ClearTextBoxesRecursive(System.Windows.Forms.Control parentControl)
        {
            foreach (System.Windows.Forms.Control control in parentControl.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
                if (control is RichTextBox)
                {
                    ((RichTextBox)control).Text = string.Empty;
                }
                if (control is ComboBox)
                {
                    ((ComboBox)control).Text = string.Empty;
                }
                else if (control.HasChildren)
                {
                    ClearTextBoxesRecursive(control); // Рекурсивный обход вложенных элементов
                }
            }
        }


        public void SavePlanPeriod()
        {
            var wbook = new XLWorkbook();
            string filePath = "simple.xlsx";
            if (!File.Exists(filePath))
            {
                var ws = wbook.Worksheets.Add("Sheet1");
                ws.Cell($"A1").Value = "Название плана";
                ws.Cell($"B1").Value = "Сумма цели";
                ws.Cell($"C1").Value = "Начальная сумма без инвестиций";
                ws.Cell($"D1").Value = "Частота взносов";
                ws.Cell($"E1").Value = "Процент дохода от инвестиций";
                ws.Cell($"F1").Value = "Сумма инвестиций";
                ws.Cell($"G1").Value = "Срок достижения цели";
                ws.Cell($"H1").Value = "Текущая годовая инфляция";
                ws.Cell($"I1").Value = "Сумма цели с учетом инфляции";
                ws.Cell($"J1").Value = "Доход от инвестиций";
                ws.Cell($"K1").Value = "Сумма регулярных взносов";
                ws.Cell($"L1").Value = "Количество взносов";
                wbook.SaveAs("simple.xlsx");

                
            }
            using (wbook = new XLWorkbook(filePath))
            {
                var ws = wbook.Worksheet(1);
                for (int i = 2; i < 100; i++)
                {
                    if (ws.Cell($"A{i}").Value.ToString() == planPeriod.name.ToString())
                    {
                        MessageBox.Show("План с заданным именем уже существует!\nУкажите уникальное имя плана");
                        break;
                    }
                    if (ws.Cell($"A{i}").IsEmpty())
                    {
                        ws.Cell($"A{i}").Value = planPeriod.name.ToString();
                        ws.Cell($"B{i}").Value = planPeriod.GoalAmount.ToString();
                        ws.Cell($"C{i}").Value = planPeriod.StartAmount.ToString();
                        ws.Cell($"D{i}").Value = planPeriod.Frequency.ToString();
                        ws.Cell($"E{i}").Value = planPeriod.incomePercent.ToString();
                        ws.Cell($"F{i}").Value = planPeriod.investAmount.ToString();
                        ws.Cell($"G{i}").Value = planPeriod.time.ToString();
                        ws.Cell($"H{i}").Value = planPeriod.inflation.ToString();
                        ws.Cell($"I{i}").Value = planPeriod.amountWithInflation.ToString();
                        ws.Cell($"J{i}").Value = planPeriod.investIncome.ToString();
                        ws.Cell($"K{i}").Value = planPeriod.PaymentAmount.ToString();
                        ws.Cell($"L{i}").Value = planPeriod.countPayments.ToString();

                        countSavedPlan++;
                        break;
                    }
                }
                wbook.Save();
            }
        }

        public void SavePlanPayment()
        {
            var wbook = new XLWorkbook();
            string filePath = "simple.xlsx";
            if (!File.Exists(filePath))
            {
                var ws = wbook.Worksheets.Add("Sheet1");
                ws.Cell($"A1").Value = "Название плана";
                ws.Cell($"B1").Value = "Сумма цели";
                ws.Cell($"C1").Value = "Начальная сумма без инвестиций";
                ws.Cell($"D1").Value = "Частота взносов";
                ws.Cell($"E1").Value = "Процент дохода от инвестиций";
                ws.Cell($"F1").Value = "Сумма инвестиций";
                ws.Cell($"G1").Value = "Срок достижения цели";
                ws.Cell($"H1").Value = "Текущая годовая инфляция";
                ws.Cell($"I1").Value = "Сумма цели с учетом инфляции";
                ws.Cell($"J1").Value = "Доход от инвестиций";
                ws.Cell($"K1").Value = "Сумма регулярных взносов";
                ws.Cell($"L1").Value = "Количество взносов";
                wbook.SaveAs("simple.xlsx");
            }
            using (wbook = new XLWorkbook(filePath))
            {
                var ws = wbook.Worksheet(1);
                for (int i = 2; i < 100; i++)
                {
                    if (ws.Cell($"A{i}").Value.ToString() == planPayments.name.ToString())
                    {
                        MessageBox.Show("План с заданным именем уже существует!\nУкажите уникальное имя плана");
                        break;
                    }
                    else if (ws.Cell($"A{i}").IsEmpty())
                    {
                        ws.Cell($"A{i}").Value = planPayments.name.ToString();
                        ws.Cell($"B{i}").Value = planPayments.GoalAmount.ToString();
                        ws.Cell($"C{i}").Value = planPayments.StartAmount.ToString();
                        ws.Cell($"D{i}").Value = planPayments.Frequency.ToString();
                        ws.Cell($"E{i}").Value = planPayments.incomePercent.ToString();
                        ws.Cell($"F{i}").Value = planPayments.InvestAmount.ToString();
                        ws.Cell($"G{i}").Value = planPayments.Time.ToString();
                        ws.Cell($"H{i}").Value = planPayments.Inflation.ToString();
                        ws.Cell($"I{i}").Value = planPayments.amountWithInflation.ToString();
                        ws.Cell($"J{i}").Value = planPayments.investIncome.ToString();
                        ws.Cell($"K{i}").Value = planPayments.paymentAmount.ToString();
                        ws.Cell($"L{i}").Value = planPayments.countPayments.ToString();


                        break;
                    }
                }
                wbook.Save();
            }
        }


        private void label24_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            plans.Add(planPayments);

            SavePlanPayment();

            Panel.AddPanel(planPayments);

            ClearAllTextBoxesInTabPage(CurrentForm);
            ClearChart2();
        }
        public void ClearChart2()
        {
            chart.Annotations.Clear();
            foreach (var series in chart.Series)
            {
                series.Points.Clear(); // Очищает точки данных, но оставляет серию
            }
        }
    }
}

