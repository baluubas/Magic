using System;
using System.Linq;
using System.Collections.Generic;
using Caliburn.Micro;
using Magic.UI;
using StructureMap;

namespace Magic.Bootstrap
{
	public class StructureMapBootstrapper : Bootstrapper<IShellViewModel>
	{
		IContainer _container;

		protected override void Configure()
		{
			var windowManager = new WindowManager();

			_container = new Container(
				x => x.Scan(
					scanner =>
				    {
					    scanner.TheCallingAssembly();
						scanner.AddAllTypesOf<IScreen>();
					    scanner.WithDefaultConventions();
				    }));

			_container.Inject(typeof(EventAggregator), new EventAggregator());
			_container.Inject(typeof(IWindowManager), windowManager);

			_container.AssertConfigurationIsValid();
		}

		protected override object GetInstance(Type serviceType, string key)
		{
			object result = null;
			if (key == null)
				result = _container.TryGetInstance(serviceType);
			else
				result = _container.TryGetInstance(serviceType, key);

			return result;
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return  _container.GetAllInstances(serviceType).Cast<object>();
		}

		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}
	}
}
