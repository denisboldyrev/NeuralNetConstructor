﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetCounstructor
{
    public partial class FormHelp : Form
    {
        public FormHelp()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
                richTextBox1.Text = 
@"Для установки программного продукта «Конструктор нейронных сетей» необходимо из каталога с программой на установочном диске скопировать все содержимое на компьютер пользователя. Дополнительных действий по инсталлированию программного продукта не требуется. Важно отметить, что для нормальной работы программы на компьютере пользователя должен быть установлен пакет библиотек .NET Framework 4.0 и выше.
Запуск программы происходит двойным нажатием левой кнопки мыши по файлу NeuralConstructor.exe. 
После запуска на экране пользователя появляется главная форма программы.
";

            if (listBox1.SelectedIndex == 1)
                richTextBox1.Text =
@"Главное окно программы состоит из рабочей области и строки меню. Рабочая область состоит из элементов ввода, в которые вво-дятся параметры создаваемой сети.
В общем виде процесс создания новой сети можно представить следующим образом:
– выбор типа сети;
– выбрать тип решаемой задачи;
– задать число входов и число слоёв для многослойной сети (для линейного и нелинейного нейрона задается только лишь число входов);
– для многослойной сети указать параметры для каждого слоя (для одного нейрона данный шаг необходимо пропустить);
– задание скорости обучения, размер обучающего множества и точность обучения.
После выполнения данных этапов запускается новая форма для решения того типа задач, который был выбран.
";
            if (listBox1.SelectedIndex == 2)
                richTextBox1.Text =
@"Линейный (нелинейный) нейрон обладает множеством входов и одним выхо-дом. С помощью нейрона можно решать задачи регрессии и классификации.
Для создания линейного нейрона в раскрывающимся списке «Тип нейронной сети» выбрать пункт «Линейный нейрон». Далее необходимо задать число входов и нажать на кнопку «Задать». Затем необходимо указать скорость обучения. Диапазон изменения скорости обучения составляет (0;1). Если пользователь неверно указал этот параметр, на экране появится предупреждающее сообщение. 
Число эпох указывает на размер обучающего множества. Точность обучения является критерием остановки обучения. Следовательно, она должна быть меньше единицы и стремиться к нулю. Следует относится внимательно к данному парамет-ру, так как при указании высокой точности обучения будет занимать более продол-женный период времени.
Кроме того, для нелинейного нейрона (персептрона) имеется возможность задавать функцию активации и ее параметры. Для этого необходимо из раскрывающе-гося списка «Функция активации» выбрать необходимый пункт и задать соответствующие ей параметры, затем нажать на кнопку «Принять».
После того как все параметры будут указаны необходимо нажать на кнопку «Создать» или выбрать соответствующий пункт меню Файл -> Создать. На экране появится новая форма. Процесс запуска обучения во всех случаях будет примерно одинаковым. Для этого первым делом нужно подготовить входные данные. В про-грамме имеется возможность загрузки исходных данных для обучения из файла, а также их ввод пользователем вручную в таблицы «Входные данные» и «Эталон».
Для загрузки данных необходимо выбрать соответствующий пункт из строки меню. Для загрузки обучающего набора необходимо выбрать пункт Действия-> За-грузить обучающий набор. Для загрузки эталонов необходимо выбрать пункт Дей-ствия -> Загрузить набор эталонов. После того как данные загрузятся в таблицу необходимо нажать на кнопку «Обучить» и запуститься процесс обучения. В ходе этого процесса пользователь может наблюдать ход обучения в графическом виде. На первом графике «Выход нейрона» пользователь может наблюдать реальный и жела-емый выходы. На втором графике «Оценка обучения сети» приводится график целе-вой функции, которую необходимо минимизировать в процессе обучения. На треть-ем графике «Ошибка» показана разница между текущим выходом и эталоном на те-кущей итерации. Внизу окна можно наблюдать счетчик времени обучения. Обучение заканчивается по достижению указанной точности. 
Принудительно можно остановить обучение, нажав на кнопку «Остановить». Результат работы можно сохранить, для этого в пункте меню необходимо выбрать пункт Файл -> Сохранить.
После обучения имеется возможность тестирования полученных результатов. Для этого подобным образом загружаются тестовые наборы входных данных, в поле «Число тестовых наборов» указывается число итераций тестирования. Запуск проис-ходит по нажатию на кнопке «Тестировать». Результат выполнения операции можно увидеть на экране в виде графиков.
Кроме этого для нелинейного нейрона имеется ряд дополнительных возмож-ностей, а именно возможность масштабирования входных и выходных данных. Для того чтобы воспользоваться данной возможностью необходимо поставить галочку в пункте «Масштабирование данных». Данный процесс полностью скрыт от пользова-теля, что позволяет избежать проблем с использование данного функционала.
";
            if (listBox1.SelectedIndex == 3)
                richTextBox1.Text =
@"«Конструктор нейронных сетей» позволяется создать многослойные сети для решения двух типов задач: регрессии и классификации образов. 
Для создания сети для классификации образов необходимо из раскрывающего-ся списка «Тип нейронной сети» выбрать пункт «Многослойный персептрон», а в раскрывающимся списке «Тип задачи» выбрать пункт «Классификация образов». Для решения задач регрессии в списке «Тип задачи» необходимо выбрать пункт «Прогнозирование». Дальнейшая настройка параметров для данных типов сетей происходит подобным образом. 
После указания типа нейронной сети и типа решаемой задачи необходимо ука-зать число слоев и число входов, данные параметры вводятся в соответствующие строки ввода. Для задания параметров необходимо нажать на кнопку «Задать».
Далее необходимо задать параметры для каждого слоя сети, для этого в рас-крывающимся списке «Слои» выбирается нужный слой. Имеется возможность ука-зать число нейронов, диапазон изменения начальных значений весовых коэффициентов, а также тип активационной функции. Для активационной функции имеется воз-можность задать ее коэффициенты, по умолчанию эти значения равны единице. При неверном задании числа нейронов, пользователь получит уведомление.
После задания параметров слоя необходимо нажать на клавишу «Применить», чтобы применить настройки. Данную операцию следует повторить для каждого слоя.
Затем необходимо указать скорость обучения, число обучающих наборов, точ-ность обучения и нажать на кнопку «Создать».
В зависимости от типа решаемой задачи запустится новая форма. Формы име-ют схожий внешний вид за исключением некоторых отличий.
Настройки сети можно сохранить в файл или загрузить из файла, для этого нужно выбрать соответствующий пункт из пункта меню «Файл».
При запуске формы создается новая таблица для входных данных. Число столбцов таблицы равно число входов, а число строк равно числу обучающих набо-ров. Для таблицы эталонов, число столбцов равно число нейронов в выходном слое. Пользователь может записать входные данные в таблицу вручную, а также загрузить из файла. Для загрузки данных из файла необходимо выбрать пункт меню Действия -> Загрузить обучающий набор или Действия -> Загрузить набор эталонов. При ошибке открытия файла, по каким-либо причинам, появляется предупреждающее сообщение.
Процесс обучения начинается после нажатия на кнопку «Обучить». Обучение длится до тех пор, пока не будет достигнута заданная точность, или пока пользова-тель не нажмет на кнопку «Остановить». После завершения процесса обучения мож-но протестировать результат работы сети. Для этого необходимо загрузить обучаю-щий набор входов и эталонов. В строке «Число тестовых наборов» необходимо ука-зать число итераций и нажать на кнопку «Создать». Тестирования начинается после нажатия на кнопку «Тестировать». Если результаты тестирования устраиваю, то их можно сохранить в файл, для этого необходимо воспользоваться командой Файл -> Сохранить.
";
            if (listBox1.SelectedIndex == 4)
                richTextBox1.Text =
@"Обучение нейронных сетей является очень ресурсоемким процессом, поэтому необходимо очень тщательно подходить к выбору архитектуры сети и настройки ее параметров. Кроме того, огромную роль оказывает аппаратное обеспечение машины, на которой будет запускаться «Конструктор нейронных сетей». Так, например, на многоядерных процессорах, процесс обучения будет выполняться горазда быстрее, чем на мобильных процессорах.
При выборе параметров первым делом необходимо обратить внимание на ко-личество слоев и количество нейронов в них. Многослойная сеть является очень мощным вычислительным механизмом, поэтому для большинства задач требуется порядка десяти нейронов в каждом слое. Не рекомендуется создавать более 3 слоев, так как это существенно замедляет обучение. Для таких сетей существуют более продвинутые методы глубинного обучения. Важно обратить внимание на скорость обучения диапазон которой изменяется (0;1). При высоких значениях этого парамет-ра, например, 0.9, скорость обучения будет достаточно высокой, однако существует возможность попасть в локальный минимум и на этап этапе произойдет паралич се-ти. Фактически желаемая точность не будет достигнута. При слишком малых значе-ниях скорость обучения будет заметно снижаться, поэтому рекомендуется начинать с высоких значений скорости обучения и снижать ее, если это будет действительно необходимо.
Более подробно следует рассмотреть процесс выбора количества нейронов в слоях сети. Как уже говорилось ранее, их число не должно быть слишком высоким, так как это замедляется процесс обучения. Кроме этого может возникнуть, так назы-ваемое, переобучение сети. Данная проблема возникает в случае, если сеть обладает чрезмерными вычислительными ресурсами, таким образом, она обучается точно по-вторять поведение, которое советовало входным обучающим данным, однако при подаче на вход переобученной сети могут происходить всплески, что говорит о не правильной интерпретации выходного сигнала.
Так как для обучения сети необходимо последовательно подавать на вход обучающие данные и сравнивать реальный выход сети с его желаемым аналогом, по-этому необходимо обратить внимание на некоторые правила по выбору обучающих данных. Первым делом необходимо обратить внимание на то, что для того, чтобы как можно лучше обучить сеть нужно обладать очень большим набором данных. Чем больше таких данных, тем лучших результатов можно добиться в процессе обу-чений. Следующим на что следует обратить внимание – это то, что выход активаци-онной функции как правило расположен от [0;1] или от [-1;1]. Поэтому необходимо принять меры для подготовки входных и выходных данных. Разработанная про-грамма позволяет шкалировать входные и выходные данные.
";
        }
    }
}
