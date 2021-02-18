using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoLotModel;
using System.Data;

namespace proiect_FECHETE_CATALIN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     enum ActionState
     {
       New, 
       Edit,
       Delete, 
       Nothing
     }
    public partial class MainWindow : Window
    { //using AutoLotModel;
       ActionState action = ActionState.Nothing;
       AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;
        CollectionViewSource customerOrdersViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));



            // Load data by setting the CollectionViewSource.Source property:
            customerViewSource.Source = ctx.Customers.Local;
            
            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));
            //customerOrdersViewSource.Source = ctx.Orders.Local;
            ctx.Customers.Load();
            ctx.Orders.Load(); ctx.Inventories.Load();
            cmbCustomers.ItemsSource = ctx.Customers.Local; 
            //cmbCustomers.DisplayMemberPath = "FirstName"; 
            cmbCustomers.SelectedValuePath = "CustId";
            cmbInventory.ItemsSource = ctx.Inventories.Local; 
            //cmbInventory.DisplayMemberPath = "Make"; 
            cmbInventory.SelectedValuePath = "CarId";
            System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            inventoryViewSource.Source = ctx.Inventories.Local;
            ctx.Inventories.Load();
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            //System.Windows.Data.CollectionViewSource inventoryViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // inventoryViewSource.Source = [generic data source]
            BindDataGrid();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;

            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = true;
            
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            custIdTextBox.IsEnabled = true;
           
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            custIdTextBox.Text = "";
            Keyboard.Focus(firstNameTextBox);
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;

            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;
            lastNameTextBox.IsEnabled = true;
            custIdTextBox.IsEnabled = true;

            
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;
            Keyboard.Focus(firstNameTextBox);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            Customer customer = null;
            /*string tempFirstName = firstNameTextBox.Text.ToString();
            string tempLastName = lastNameTextBox.Text.ToString();

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;
            customerDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = false;
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;

            /*BindingOperations.ClearBinding(txtPhoneNumber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtSubscriber, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtContractValue, TextBox.TextProperty);
            BindingOperations.ClearBinding(txtContractDate, TextBox.TextProperty);
            firstNameTextBox.Text = tempFirstName;
            lastNameTextBox.Text = tempLastName;*/
            try
            { customer = (Customer)customerDataGrid.SelectedItem;
            ctx.Customers.Remove(customer);

            ctx.SaveChanges();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            customerViewSource.View.Refresh();
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            customerDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    customer = new Customer()
                    {
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim()
                    };
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;

            }
            else
                if (action == ActionState.Edit)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                customerViewSource.View.MoveCurrentTo(customer);
                /*if (customer != null)
                {
                    customer.SelectedIndex = queryPhoneNumbers.ToList().FindIndex(p => p.Id == phoneNumber.Id);
                }*/
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                //lstPhones.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
                custIdTextBox.IsEnabled = false;
                /*firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameColumn);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtContractValueBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtContractDateBinding);*/
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;
                customerDataGrid.IsEnabled = true;
                //lstPhones.IsEnabled = true;
                btnPrevious.IsEnabled = true;
                btnNext.IsEnabled = true;
                firstNameTextBox.IsEnabled = false;
                lastNameTextBox.IsEnabled = false;
                custIdTextBox.IsEnabled = false;
                /*txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtContractValueBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtContractDateBinding);*/
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            customerDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = true;
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
            firstNameTextBox.IsEnabled = false;
            lastNameTextBox.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;

            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = true;
            btnPrevious1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            carIdTextBox.IsEnabled = true;

            makeTextBox.Text = "";
            colorTextBox.Text = "";
            carIdTextBox.Text = "";
            Keyboard.Focus(makeTextBox);
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempMake = makeTextBox.Text.ToString();
            string tempColor = colorTextBox.Text.ToString();

            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;

            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;
            inventoryDataGrid.IsEnabled = true;
            btnPrevious1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            makeTextBox.IsEnabled = true;
            colorTextBox.IsEnabled = true;
            carIdTextBox.IsEnabled = true;

            makeTextBox.Text = tempMake;
            colorTextBox.Text = tempColor;
            Keyboard.Focus(makeTextBox);
        }

        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                try
                {
                    inventory = new Inventory()
                    {
                        Make = makeTextBox.Text.Trim(),
                        Color = colorTextBox.Text.Trim()
                    };
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                btnPrevious1.IsEnabled = true;
                btnNext1.IsEnabled = true;

            }
            else
                if (action == ActionState.Edit)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.Make = makeTextBox.Text.Trim();
                    inventory.Color = colorTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                inventoryViewSource.View.MoveCurrentTo(inventory);
                /*if (customer != null)
                {
                    customer.SelectedIndex = queryPhoneNumbers.ToList().FindIndex(p => p.Id == phoneNumber.Id);
                }*/
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevious1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
                carIdTextBox.IsEnabled = false;
                /*firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameColumn);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtContractValueBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtContractDateBinding);*/
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevious1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                makeTextBox.IsEnabled = false;
                colorTextBox.IsEnabled = false;
                carIdTextBox.IsEnabled = false;
                /*txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtContractValueBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtContractDateBinding);*/
            }
        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew1.IsEnabled = true;
            btnEdit1.IsEnabled = true;
            btnDelete1.IsEnabled = true;
            btnSave1.IsEnabled = false;
            btnCancel1.IsEnabled = false;
            inventoryDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = true;
            btnPrevious1.IsEnabled = true;
            btnNext1.IsEnabled = true;
            makeTextBox.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;
        }

        private void btnPrevious1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        private void btnINext1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnIDelete1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            Inventory inventory = null;
            try
            {
                inventory = (Inventory)inventoryDataGrid.SelectedItem;
                ctx.Inventories.Remove(inventory);
                ctx.SaveChanges();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            inventoryViewSource.View.Refresh();
            btnNew1.IsEnabled = true;
            btnEdit1.IsEnabled = true;
            btnDelete1.IsEnabled = true;
            btnSave1.IsEnabled = false;
            btnCancel1.IsEnabled = false;
            inventoryDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = true;
            btnPrevious1.IsEnabled = true;
            btnNext1.IsEnabled = true;
            makeTextBox.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;
        }
        private void btnNew2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;

            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;
            ordersDataGrid.IsEnabled = true;
            btnPrevious2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            custIdTextBox.IsEnabled = true;
            //orderIdTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = true;

            carIdTextBox.Text = "";
            custIdTextBox.Text = "";
            //OrderIdTextBox.Text = "";
            Keyboard.Focus(carIdTextBox);
        }

        private void btnEdit2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            string tempCarId = carIdTextBox.Text.ToString();
            string tempCustId = custIdTextBox.Text.ToString();

            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;

            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;
            ordersDataGrid.IsEnabled = true;
            btnPrevious2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            carIdTextBox.IsEnabled = true;
            custIdTextBox.IsEnabled = true;
            carIdTextBox.IsEnabled = true;

            carIdTextBox.Text = tempCarId;
            custIdTextBox.Text = tempCustId;
            Keyboard.Focus(carIdTextBox);
        }

        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem; Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //instantiem Order 
                     order = new Order()
                    {
                        CustId = customer.CustId, CarId = inventory.CarId
                    }
                       
                    
                    ; //adaugam entitatea nou creata in context ctx.Orders.Add(order);
                customerOrdersViewSource.View.Refresh();
                //salvam modificarile
                ctx.SaveChanges();
                }
               catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnCancel2.IsEnabled = false;
                btnPrevious2.IsEnabled = true;
                btnNext2.IsEnabled = true;

            }
            else
                if (action == ActionState.Edit)
            {
                try
                {
                    order = (Order)ordersDataGrid.SelectedItem;
                    //order.CarId = carIdTextBox.Text.Trim();
                    //order.CustId = custIdTextBox.Text.Trim();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
                inventoryViewSource.View.MoveCurrentTo(order);
                /*if (customer != null)
                {
                    customer.SelectedIndex = queryPhoneNumbers.ToList().FindIndex(p => p.Id == phoneNumber.Id);
                }*/
                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnDelete2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnCancel2.IsEnabled = false;
                ordersDataGrid.IsEnabled = true;
                btnPrevious2.IsEnabled = true;
                btnNext2.IsEnabled = true;
                carIdTextBox.IsEnabled = false;
                custIdTextBox.IsEnabled = false;
                carIdTextBox.IsEnabled = false;
                /*firstNameTextBox.SetBinding(TextBox.TextProperty, firstNameColumn);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtContractValueBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtContractDateBinding);*/
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    order= (Order)ordersDataGrid.SelectedItem;
                    ctx.Orders.Remove(order);

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //ordersViewSource.View.Refresh();
                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnDelete2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnCancel2.IsEnabled = false;
                inventoryDataGrid.IsEnabled = true;
                btnPrevious2.IsEnabled = true;
                btnNext2.IsEnabled = true;
                custIdTextBox.IsEnabled = false;
                carIdTextBox.IsEnabled = false;
                carIdTextBox.IsEnabled = false;
                /*txtPhoneNumber.SetBinding(TextBox.TextProperty, txtPhoneNumberBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtSubscriberBinding);
                txtPhoneNumber.SetBinding(TextBox.TextProperty, txtContractValueBinding);
                txtSubscriber.SetBinding(TextBox.TextProperty, txtContractDateBinding);*/
            }
        }

        private void btnCancel2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew2.IsEnabled = true;
            btnEdit2.IsEnabled = true;
            btnDelete2.IsEnabled = true;
            btnSave2.IsEnabled = false;
            btnCancel2.IsEnabled = false;
            ordersDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = true;
            btnPrevious2.IsEnabled = true;
            btnNext2.IsEnabled = true;
            carIdTextBox.IsEnabled = false;
            custIdTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;
        }

        private void btnPrevious2_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }

        private void btnINext2_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnIDelete2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
            Inventory inventory = null;
            try
            {
                inventory = (Inventory)inventoryDataGrid.SelectedItem;
                ctx.Inventories.Remove(inventory);
                ctx.SaveChanges();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            customerViewSource.View.Refresh();
            btnNew2.IsEnabled = true;
            btnEdit2.IsEnabled = true;
            btnDelete2.IsEnabled = true;
            btnSave2.IsEnabled = false;
            btnCancel2.IsEnabled = false;
            inventoryDataGrid.IsEnabled = true;
            //lstPhones.IsEnabled = true;
            btnPrevious2.IsEnabled = true;
            btnNext2.IsEnabled = true;
            makeTextBox.IsEnabled = false;
            colorTextBox.IsEnabled = false;
            carIdTextBox.IsEnabled = false;
        }
        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.CarId
                 equals inv.CarId
                             select new
                             {
                                 ord.OrderId,
                                 ord.CarId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.Make,
                                 inv.Color
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }

        private void inventoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    }


            
            

