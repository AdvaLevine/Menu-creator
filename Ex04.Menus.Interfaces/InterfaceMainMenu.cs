﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ex04.Menus.Interfaces
{
    public class InterfaceMainMenu
    {
        private const int k_GoBackOrExit = 0;
        private InterfaceSubMenu m_MainMenu;

        public InterfaceMainMenu(string i_Title)
        {
            m_MainMenu = new InterfaceSubMenu(i_Title);
        }

        public void AddMainMenuItem(InterfaceMenuItem i_MenuItem)
        {
            m_MainMenu.AddSubMenuItem(i_MenuItem);
        }

        private bool isUserInputInRange(int i_InputFromUser, InterfaceMenuItem i_MenuItem)
        {
            return (i_InputFromUser >= 0 && i_InputFromUser <= (i_MenuItem as InterfaceSubMenu).SubMenu.Count);
        }

        public void Show()
        {
            InterfaceMenuItem currentMenuItem = m_MainMenu;

            while (currentMenuItem != null)
            {
                currentMenuItem.MenuExecuteHandler();
                goBackIfAction(ref currentMenuItem);
                try
                {
                    updateMenuItemsFromUser(ref currentMenuItem);
                }
                catch (ValueOutOfRangeException valueOutOfRange)
                {
                    Console.WriteLine(valueOutOfRange.Message);
                    promptUserToPressKeyToContinue();
                }
                catch (FormatException formatException)
                {
                    Console.WriteLine(formatException.Message);
                    promptUserToPressKeyToContinue();
                }
            }
        }
        private void goBackIfAction(ref InterfaceMenuItem io_CurrentMenuItem)
        {
            if (io_CurrentMenuItem.IsActionMenuItem())
            {
                goBack(ref io_CurrentMenuItem);
                io_CurrentMenuItem.MenuExecuteHandler();
            }
        }

        private void goBack(ref InterfaceMenuItem io_CurrentMenuItem)
        {
            io_CurrentMenuItem = io_CurrentMenuItem.Parent;
        }

        private void updateMenuItemsFromUser(ref InterfaceMenuItem io_CurrentMenuItem)
        {
            Console.WriteLine("Enter your request: (1 to {0} or press '0' to {1})", (io_CurrentMenuItem as InterfaceSubMenu).SubMenu.Count, (io_CurrentMenuItem as InterfaceSubMenu).GetLastMenuOption());
            if (int.TryParse(Console.ReadLine(), out int userInput))
            {
                if (!isUserInputInRange(userInput, io_CurrentMenuItem))
                {
                    throw new ValueOutOfRangeException(0, (io_CurrentMenuItem as InterfaceSubMenu).SubMenu.Count);
                }

                if (userInput == k_GoBackOrExit)
                {
                    goBack(ref io_CurrentMenuItem);
                }
                else
                {
                    Console.Clear();
                    goToUserChoice(ref io_CurrentMenuItem, userInput);
                }
            }
            else
            {
                throw new FormatException("Invalid input, must be a number.");
            }
        }

        private void goToUserChoice(ref InterfaceMenuItem io_CurrentMenuItem, int i_UserInput)
        {
            if (io_CurrentMenuItem is InterfaceSubMenu)
            {
                io_CurrentMenuItem = (io_CurrentMenuItem as InterfaceSubMenu).GetMenuItemByIndex(i_UserInput);
            }
        }

        private void promptUserToPressKeyToContinue()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
