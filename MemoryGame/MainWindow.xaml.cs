using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer(); // determine how long 2 cards which are choosen will be shown
        Random random = new Random(); 
        List<int> numbers = new List<int>() // numbers behind the cards
        {
            0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14,15,15,16,16,17,17,18,18,19,19,20,20,21,21,22,22,23,23,24,24
        };
        int clickCounter = 0; // count how many times it has been clicked
        int matchCounter = 0; // keep how many times 2 cards have same numbers
        Button firstClicked, secondClicked;

    public MainWindow()
        {
            InitializeComponent();
            AssignNumbersToBoxes(); // assign numbers from list to cards

        }
        private void AssignNumbersToBoxes()
        {
            List<int> numberscopy = new List<int>(numbers); // create copy list in order to reuse again in Reset Function
            Button button;
            int randomNumb;
            for(int i=0 ; i< grid1.Children.Count; i++) // repeat up to how many elements(cards) are inside grid
            {
                if (grid1.Children[i] is Button) // check if it is a Button
                {
                    button = (Button)grid1.Children[i];
                }
                else
                    continue;

                randomNumb = random.Next(0, numberscopy.Count); // select a random number from how many values are in List
                button.Content = numberscopy[randomNumb]; // put one of the list values into button's content

                numberscopy.RemoveAt(randomNumb); // remove the element which is assigned to button's content from list elements
            }
        }

        private void button_click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            clickCounter++; // increase number when button clicked
            attempt.Text = "Attempts: " + clickCounter; // attempt is the textblock in the program,here show counter number with it
            attempt.UpdateLayout();

            if (firstClicked != null && secondClicked != null)
                return;
               
            if (clickedButton == null)
                return;

            if (clickedButton.Foreground == Brushes.White) // it means card clicked before so,it's open already
                return;

            if(firstClicked == null)
            {
                firstClicked = clickedButton; // assign button which is clicked to firstClicked
                firstClicked.Foreground = Brushes.White;
                firstClicked.Background = Brushes.Orange;
                return;
            }

            secondClicked = clickedButton; // if firstClicked has already value, the other click assigns to secondClicked
            secondClicked.Foreground = Brushes.White;
            secondClicked.Background = Brushes.Orange;

            if (firstClicked.Content.ToString() == secondClicked.Content.ToString()) // when their contents equal each other
            {
                matchCounter++; // it's found 2 same cards
                getSol.Text = "Solved: " + matchCounter; // getSol is textBlock in the program("Solved:"),here show counter number with it
                firstClicked = null;
                secondClicked = null;

            }
            else
            {
                timer.Tick += Timer_tick;
                timer.Interval = new TimeSpan(0, 0, 0, 0,750);  // starts to run for 750 ms
                timer.Start();
            }

        }
        private void Timer_tick(object sender,EventArgs e)
        {
                timer.Stop();

            if (firstClicked == null) // it requires to be choosen to do operations below
                return;
            else                                
            {
                firstClicked.Foreground = Brushes.Transparent; // turn back first card
                firstClicked.Background = Brushes.LightGray;
                secondClicked.Foreground = Brushes.Transparent; // turn back second card
                secondClicked.Background = Brushes.LightGray;


                firstClicked = null;
                secondClicked = null;
            }  
        }

        private void Reset_Click(object sender, RoutedEventArgs e) // restart game
        {
            attempt.Text = "Attempts: 0"; //refresh     
            getSol.Text = "Solved: 0"; // refresh

            AssignNumbersToBoxes(); // reassign numbers to cards

            clickCounter = 0;
            matchCounter = 0;
            timer.Stop();

            foreach (Button button in grid1.Children)
            {
                button.Foreground = Brushes.Transparent; // turn back cards
            }
        }
    }
}
