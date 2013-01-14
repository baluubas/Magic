using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Magic.UI.SelectFigures.Views
{
	public delegate void FigureSelectedEventHandler(object sender, FigureSelectedEventArgs args);
	public delegate void FiguresUpdatedEventHandler(object sender, PageRotatedEventArgs args);

	public class SelectionCanvas : Canvas
	{
		private static int NextSelectionId;

		private Point _mouseLeftDownPoint;
		private Selection _currentSelection;
		private FigureSelectedEventArgs _currentSelectionEventArgs;
		private double _currentAngle = 0;

		public static readonly RoutedEvent FigureSelectedEvent;
		public static readonly RoutedEvent FiguresUpdatedEvent;

		public event FigureSelectedEventHandler FigureSelected
		{
			add { AddHandler(FigureSelectedEvent, value); }
			remove { RemoveHandler(FigureSelectedEvent, value); }
		}

		public event FiguresUpdatedEventHandler FigureUpdated
		{
			add { AddHandler(FiguresUpdatedEvent, value); }
			remove { RemoveHandler(FiguresUpdatedEvent, value); }
		}

		static SelectionCanvas()
		{
			FigureSelectedEvent =
				EventManager.RegisterRoutedEvent("FigureSelected", RoutingStrategy.Bubble, typeof(FigureSelectedEventHandler), typeof(SelectionCanvas));
			FiguresUpdatedEvent =
				EventManager.RegisterRoutedEvent("FigureUpdated", RoutingStrategy.Bubble, typeof(FiguresUpdatedEventHandler), typeof(SelectionCanvas));
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			SizeChanged += OnSizeChanged;
		}

		public void Rotate(Image image)
		{
			TransformedBitmap source = (TransformedBitmap)image.Source;
			var oldDimensions = new Size(ActualWidth, ActualHeight);

			var rotateTransform = ((RotateTransform) source.Transform);
			rotateTransform.Angle += 90;
			image.Source = source.Clone();

			_currentAngle = rotateTransform.Angle;
			UpdateSelections90Degrees(oldDimensions);
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (!IsMouseCaptured)
			{
				_mouseLeftDownPoint = e.GetPosition(this);
				CaptureMouse();
			}
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);

			if (IsMouseCaptured && _currentSelectionEventArgs != null)
			{
				ReleaseMouseCapture();

				RaiseEvent(_currentSelectionEventArgs);
				_currentSelection = null;
				_currentSelectionEventArgs = null;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (IsMouseCaptured)
			{
				if (_currentSelection == null)
				{
					var newSelection = new Selection(NextSelectionId++);
					Children.Add(newSelection);
					_currentSelectionEventArgs = new FigureSelectedEventArgs(
						FigureSelectedEvent,
						this,
						newSelection.Id,
						() => Children.Remove(newSelection));
					
					_currentSelection = newSelection;
				}

				Point currentPoint = e.GetPosition(this);
				Point modifiedPoint = RestrictToCanvasBounds(currentPoint);

				double width = Math.Abs(_mouseLeftDownPoint.X - modifiedPoint.X);
				double height = Math.Abs(_mouseLeftDownPoint.Y - modifiedPoint.Y);
				double left = Math.Min(_mouseLeftDownPoint.X, modifiedPoint.X);
				double top = Math.Min(_mouseLeftDownPoint.Y, modifiedPoint.Y);

				_currentSelection.Width = width;
				_currentSelection.Height = height;
				SetLeft(_currentSelection, left);
				SetTop(_currentSelection, top);

				_currentSelectionEventArgs.RelativeOffsetY = top / ActualHeight;
				_currentSelectionEventArgs.RelativeOffsetX = left / ActualWidth;
				_currentSelectionEventArgs.RelativeWidth = width / ActualWidth;
				_currentSelectionEventArgs.RelativeHeight = height / ActualHeight;
			}
		}

		private Point RestrictToCanvasBounds(Point currentPoint)
		{
			var result = new Point(currentPoint.X, currentPoint.Y);

			if (result.X < 0)
			{
				result.X = 0;
			}
			else if (result.X > ActualWidth)
			{
				result.X = ActualWidth;
			}

			if (result.Y < 0)
			{
				result.Y = 0;
			}
			else if (result.Y > ActualHeight)
			{
				result.Y = ActualHeight;
			}

			return result;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			double scale;
			if (e.HeightChanged)
			{
				scale = e.NewSize.Height / e.PreviousSize.Height;
			}
			else
			{
				scale = e.NewSize.Width/e.PreviousSize.Height;
			}

			foreach (Selection selection in Children)
			{
				var oldRect = new Rect(GetLeft(selection), GetTop(selection), selection.Width, selection.Height);
				var newRect = new Rect(
					oldRect.X*scale,
					oldRect.Y*scale,
					oldRect.Width*scale,
					oldRect.Height*scale);

				selection.Width = newRect.Width;
				selection.Height = newRect.Height;
				SetLeft(selection, newRect.X);
				SetTop(selection, newRect.Y);
			}
		}

		private void UpdateSelections90Degrees(Size oldDim)
		{
			foreach (Selection selection in Children)
			{
				var oldRect = new Rect(GetLeft(selection), GetTop(selection), selection.Width, selection.Height);
				var newRect = new Rect(
					oldDim.Height - oldRect.Y - oldRect.Height,
					oldRect.X,
					oldRect.Height,
					oldRect.Width);

				selection.Width = newRect.Width;
				selection.Height = newRect.Height;
				SetLeft(selection, newRect.X);
				SetTop(selection, newRect.Y);

				RaiseEvent(new PageRotatedEventArgs(FiguresUpdatedEvent, this, selection.Id, (int)_currentAngle));
			}
		}
	}
}
