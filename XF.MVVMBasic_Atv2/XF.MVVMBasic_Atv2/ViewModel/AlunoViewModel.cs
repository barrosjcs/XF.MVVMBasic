using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using XF.MVVMBasic.Model;
using XF.MVVMBasic.View;
using XF.MVVMBasic_Atv2;

namespace XF.MVVMBasic.ViewModel
{
    public class AlunoViewModel
    {
        #region Propriedades
        public Aluno AlunoModel { get; set; }
        public ObservableCollection<Aluno> Alunos { get; set; }
        
        public OnAddAluno OnAddAluno { get; }
        public OnExcludeAluno OnExcludeAluno { get; }
        public ICommand OnNewCommand { get; private set; }
        public ICommand OnCloseCommand { get; private set; }
        public ICommand OnOutCommand { get; private set; }
        #endregion

        public AlunoViewModel()
        {
            Alunos = new ObservableCollection<Aluno>();
            OnAddAluno = new OnAddAluno(this);
            OnExcludeAluno = new OnExcludeAluno(this);
            OnNewCommand = new Command(OnNew);
            OnOutCommand = new Command(OnOut);
            OnCloseCommand = new Command(OnClose);
        }

        public void Adicionar(Aluno paramAluno)
        {
            try
            {
                if (paramAluno == null)
                    throw new NullReferenceException("Aluno não encontrado");

                paramAluno.Id = Guid.NewGuid();
                Alunos.Add(paramAluno);
                Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async void Remover(Aluno paramAluno)
        {
            bool removerAluno = await Application.Current.MainPage.DisplayAlert("Remover"
                , string.Format("Tem certeza que deseja remover o aluno: {0}"
                , paramAluno.Nome)
                , "Sim", "Não");

            try
            {
                if (removerAluno)
                {
                    Alunos.Remove(paramAluno);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnOut() => await Application.Current.MainPage.Navigation.PopAsync();

        private async void OnNew()
        {
            App.AlunoVM.AlunoModel = new Aluno();
            await Application.Current.MainPage.Navigation.PushAsync(new NovoAlunoView()
            {
                BindingContext = App.AlunoVM
            });
        }

        private void OnClose(object obj)
        {
            Application.Current.Quit();
        }
    }

    public class OnAddAluno : ICommand
    {
        private readonly AlunoViewModel alunoVM;
        public OnAddAluno(AlunoViewModel paramVM)
        {
            alunoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void IsEnabledExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => parameter != null && !string.IsNullOrWhiteSpace(((Aluno)parameter).Nome);
        public void Execute(object parameter)
        {
            alunoVM.Adicionar((Aluno)parameter);
        }
    }

    public class OnExcludeAluno : ICommand
    {
        private readonly AlunoViewModel alunoVM;
        public OnExcludeAluno(AlunoViewModel paramVM)
        {
            alunoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => parameter != null;
        public void Execute(object parameter)
        {
            alunoVM.Remover((Aluno)parameter);
        }
    }
}
