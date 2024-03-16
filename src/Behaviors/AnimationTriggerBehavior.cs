using System.ComponentModel;
using AlohaKit.Animations;
using CatMatch.ViewModels;

namespace CatMatch.Behaviors;

public class AnimationTriggerBehavior : Behavior<View>
{
    private View AttachedView;
    
    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);
        AttachedView = bindable;
        bindable.BindingContextChanged += OnBindingContextChanged;
    }

    protected override void OnDetachingFrom(View bindable)
    {
        if (bindable.BindingContext is INotifyPropertyChanged viewModel)
        {
            viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }
        bindable.BindingContextChanged -= OnBindingContextChanged;
        base.OnDetachingFrom(bindable);
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (sender is View view && view.BindingContext is INotifyPropertyChanged viewModel)
        {
            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CatCardViewModel.ShowAnimation) && sender is CatCardViewModel cvm)
        {
            AttachedView.Animate(new FlipAnimation { Duration = "300" });
        }
    }
}
