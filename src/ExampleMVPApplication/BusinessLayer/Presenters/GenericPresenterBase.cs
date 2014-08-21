using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleMVPApplication.Interfaces;

namespace ExampleMVPApplication.BusinessLayer.Presenters
{
    /// <summary>
    /// GenericPresenterBase class
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    public abstract class GenericPresenterBase<TView> where TView : IView
    {
        private TView _View;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericPresenterBase{TView}" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected GenericPresenterBase(TView view)
        {
            this._View = view;
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public TView View
        {
            get
            {
                return this._View;
            }
        }

        /// <summary>
        /// Adds any errors to the view if it implements IErrorView.
        /// </summary>
        /// <param name="errors">The errors.</param>
        internal void AddErrors(IList<string> errors)
        {
            IErrorView view = this.View as IErrorView;

            if (view != null)
            {
                foreach (var errorMessage in errors.Select(ent => ent).Distinct().ToList())
                {
                    view.AddError(errorMessage);
                }
            }
        }
    }
}
