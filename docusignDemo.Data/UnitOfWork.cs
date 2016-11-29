using System;
using docusignDemo.Data.Entities;


namespace docusignDemo.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly docusignDemoEntities _entity = new docusignDemoEntities();
 
        
        private GenericRepository<Template> _templatesRepository;
        private GenericRepository<TemplateRole> _templateRolesRepository;




        private string _ex;
        private bool _hasError;
        public UnitOfWork()
        { }

        public UnitOfWork(bool proxyCreationEnabled)
        {
            _entity.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }




        public GenericRepository<Template> TemplatesRepository
        {
            get
            {

                if (this._templatesRepository == null)
                {
                    this._templatesRepository = new GenericRepository<Template>(_entity);
                }
                return _templatesRepository;
            }
        }

        public GenericRepository<TemplateRole> TemplateRolesRepository
        {
            get
            {

                if (this._templateRolesRepository == null)
                {
                    this._templateRolesRepository = new GenericRepository<TemplateRole>(_entity);
                }
                return _templateRolesRepository;
            }
        }















        public void Save()
        {
            try
            {
                _ex = null;
                _hasError = false;

                _entity.SaveChanges();
            }
            catch (Exception ex)
            {
                _ex = ex.InnerException?.InnerException?.Message ?? ex.Message;
                _hasError = true;
            }
        }
        public string GetError()
        {
            return _ex;
        }
        public bool HasError()
        {
            return _hasError;
        }



        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _entity.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
