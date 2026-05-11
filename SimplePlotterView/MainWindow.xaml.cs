using Auxiliary;
using OxyPlot;
using SimplePlotterMisc;
using SimplePlotterVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Xaml;
using System.Xml.Linq;

namespace SimplePlotterView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new SimplePlotterVM.VM();
            InitializeComponent();
            ((SimplePlotterVM.VM)DataContext).PropertyChanged += MainWindow_PropertyChanged;
        }

        void MainWindow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Ex")
            {
                SimplePlotterVM.VM vm = (SimplePlotterVM.VM)DataContext;
                MessageBox.Show(vm.Ex.Message);
            }
            else if (e.PropertyName == "InterfaceLanguage")
            {
                foreach (var item in findVisualChildren<ComboBox>(this))
                {
                    ComboBox cb = item as ComboBox;
                    if (cb != null)
                    {
                        object dc = cb.DataContext;
                        cb.DataContext = null;
                        cb.DataContext = dc;
                    }
                }
            }
            if (e.PropertyName == "Question")
            {
                SimplePlotterVM.VM vm = (SimplePlotterVM.VM)DataContext;
                vm.Answer = MessageBox.Show(vm.Question, "", MessageBoxButton.YesNo);
            }
        }

        #region MISC

        private void txtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBx = sender as TextBox;
            txtBx.SelectAll();
        }

        private void lvItem_GotFocus(object sender, RoutedEventArgs e)
        {
            //try TextBox
            TextBox txtBx = sender as TextBox;
            ListView lv = GetParent(txtBx);
            if (txtBx != null)
            {
                lv.SelectedItem = (sender as TextBox).DataContext;
                return;
            }
            else
            {
                //try ComboBox
                ComboBox cmbBx = sender as ComboBox;
                lv = GetParent(cmbBx);
                if (cmbBx != null)
                {
                    lv.SelectedItem = (sender as ComboBox).DataContext;
                    return;
                }
                else
                {
                    //try CheckBox
                    CheckBox chBx = sender as CheckBox;
                    lv = GetParent(chBx);
                    if (chBx != null)
                    {
                        lv.SelectedItem = (sender as CheckBox).DataContext;
                        return;
                    }
                }
            }
        }

        private ListView GetParent(Visual v)
        {
            while (v != null)
            {
                v = VisualTreeHelper.GetParent(v) as Visual;
                if (v is ListView)
                    break;
            }
            return v as ListView;
        }

        private string getCurrentVersion()
        {
            string version = getClickOnceVersion();
            if (null == version)
            {
                version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return version;
        }

        private string getClickOnceVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            return null;
        }

        public static IEnumerable<T> findVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in findVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public T getAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)getAncestorOfType<T>((FrameworkElement)parent);
            return (T)parent;
        }

        //private void lv_EnterPressed(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        TextBox tb = (TextBox)sender;
        //        ListViewItem lvi = getAncestorOfType<ListViewItem>(tb);
        //        ListView lv = getAncestorOfType<ListView>(lvi);
        //        bool shiftPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
        //        int currentSelected = lv.SelectedIndex;
        //        if (shiftPressed && currentSelected == 0) return;
        //        if (!shiftPressed && currentSelected == lv.Items.Count - 1) return;
        //        int total = lv.Items.Count;
        //        if (currentSelected < total)
        //        {
        //            BindingExpression be = tb.GetBindingExpression(TextBox.TextProperty);
        //            ListViewItem nlvi = (ListViewItem)lv.ItemContainerGenerator.ContainerFromIndex(currentSelected + (shiftPressed ? -1 : 1));
        //            foreach (var txtBx in findVisualChildren<TextBox>(lv))
        //            {
        //                BindingExpression nbe = txtBx.GetBindingExpression(TextBox.TextProperty);
        //                if (txtBx.DataContext == nlvi.DataContext && be.ResolvedSourcePropertyName == nbe.ResolvedSourcePropertyName)
        //                {
        //                    Keyboard.Focus(txtBx);
        //                    txtBx.SelectAll();
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //}

        // Pending to change this method to a non VM dependent, but listview.SelectedItems is tough
        private void lv_EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = (TextBox)sender;
                ListViewItem lvi = getAncestorOfType<ListViewItem>(tb);
                ListView lv = getAncestorOfType<ListView>(lvi);
                if (lv.SelectedItems.Count > 1)
                {
                    VM dc = lv.DataContext as VM;
                    dc.LongProcessRuning = true;
                    string newValue = tb.Text;
                    BindingExpression be = tb.GetBindingExpression(TextBox.TextProperty);
                    foreach (DataSeriesObj ds in lv.SelectedItems)
                    {
                        Type type = typeof(DataSeriesObj);
                        PropertyInfo myPropertyInfo = type.GetProperty(be.ResolvedSourcePropertyName);
                        if (myPropertyInfo != null)
                        {
                            myPropertyInfo.SetValue(ds, Convert.ToDouble(newValue), null);
                        }
                    }
                    dc.LongProcessRuning = false;
                    return;
                }
                else
                {
                    bool shiftPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift);
                    int currentSelected = lv.SelectedIndex;
                    if (shiftPressed && currentSelected == 0) return;
                    if (!shiftPressed && currentSelected == lv.Items.Count - 1) return;
                    int total = lv.Items.Count;
                    if (currentSelected < total)
                    {
                        BindingExpression be = tb.GetBindingExpression(TextBox.TextProperty);
                        ListViewItem nlvi = (ListViewItem)lv.ItemContainerGenerator.ContainerFromIndex(currentSelected + (shiftPressed ? -1 : 1));
                        foreach (var txtBx in findVisualChildren<TextBox>(lv))
                        {
                            BindingExpression nbe = txtBx.GetBindingExpression(TextBox.TextProperty);
                            if (txtBx.DataContext == nlvi.DataContext && be.ResolvedSourcePropertyName == nbe.ResolvedSourcePropertyName)
                            {
                                Keyboard.Focus(txtBx);
                                txtBx.SelectAll();
                                return;
                            }
                        }
                    }
                }
            }
        }

        // Pending to change this method to a non VM dependent, but listview.SelectedItems is tough
        private void lv_DeletePressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ListView lv = (ListView)sender;
                VM dc = lv.DataContext as VM;
                foreach (var item in lv.SelectedItems)
                {
                    dc.SelectedDataSeriesExtended.Add((DataSeriesObj)item);
                }
                DelegateCommand delC = dc.RemoveDataSeriesExtended;
                delC.Execute(true);
                dc.SelectedDataSeriesExtended.Clear();
            }
        }

        #endregion

        private void SelectAllDataSeries_Click(object sender, RoutedEventArgs e)
        {
            lvDataSeries.SelectAll();
        }
    }
}
