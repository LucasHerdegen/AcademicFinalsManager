import 'bootstrap/dist/css/bootstrap.min.css';

export const Register = () => {
  return (
    <div className="container mt-5">
      <div className="card shadow-sm mx-auto" style={{ maxWidth: '500px' }}>
        <div className="card-body p-4">
          <h2 className="text-center mb-4">Registrate!</h2>
          
          <form>
            <div className="row mb-3">
              <div className="col-12">
                <label className="form-label">Nombre</label>
                <input type="text" className="form-control" placeholder="Tu nombre" />
              </div>
            </div>

            <div className="row mb-3">
              <div className="col-12">
                <label className="form-label">Apellido</label>
                <input type="text" className="form-control" placeholder="Tu apellido" />
              </div>
            </div>

            <div className="row mb-3">
              <div className="col-12">
                <label className="form-label">Correo electrónico</label>
                <input type="email" className="form-control" placeholder="nombre@ejemplo.com" />
              </div>
            </div>

            <div className="row mb-4">
              <div className="col-12">
                <label className="form-label">Contraseña</label>
                <input type="password" name="password" className="form-control" placeholder="********" />
              </div>
            </div>

            <div className="row">
              <div className="col-12 d-grid">
                <button type="submit" className="btn btn-primary">
                  Registrarse
                </button>
              </div>
            </div>
          </form>
          
        </div>
      </div>
    </div>
  );
};
