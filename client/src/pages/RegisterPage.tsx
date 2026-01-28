import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { Home } from 'lucide-react'

export default function RegisterPage() {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    fullName: '',
    phone: '',
    location: '',
  })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const { register } = useAuth()
  const navigate = useNavigate()

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      await register(formData.email, formData.password, formData.fullName, formData.phone || undefined, formData.location || undefined)
      navigate('/download-app')
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al registrar usuario')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center px-4 py-8">
      <div className="max-w-2xl w-full bg-white rounded-2xl shadow-2xl p-10 border border-black mx-auto">
        {/* Botón Volver al Inicio */}
        <div className="mb-6">
          <Link
            to="/"
            className="inline-flex items-center gap-2 text-lg text-black font-semibold hover:text-gray-700 transition-all"
          >
            <Home className="h-5 w-5" />
            Volver al Inicio
          </Link>
        </div>
        
        {/* Header Estructurado */}
        <div className="text-center mb-10 border-b-2 border-black pb-6">
          <div className="inline-flex items-center justify-center w-24 h-24 bg-white rounded-2xl mb-6 shadow-xl border border-black overflow-hidden p-2">
            <img src={`${import.meta.env.BASE_URL}images/logo_front.jpeg`} alt="Cownect Logo" className="w-full h-full object-contain max-w-full max-h-full" />
          </div>
          <h1 className="text-4xl font-bold text-black mb-2">Cownect</h1>
          <div className="w-24 h-1 bg-black mx-auto mb-3"></div>
          <p className="text-xl text-black font-semibold">Registro de Usuario</p>
          <p className="text-lg text-black mt-2 font-normal">Complete el formulario con sus datos para crear su cuenta de acceso</p>
        </div>

        {error && (
          <div className="mb-6 p-4 bg-red-100 border border-red-500 text-red-900 rounded-xl text-lg font-bold">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <label htmlFor="fullName" className="block text-xl font-bold text-black mb-2">
              Nombre Completo
            </label>
            <input
              id="fullName"
              name="fullName"
              type="text"
              value={formData.fullName}
              onChange={handleChange}
              required
              className="w-full px-6 py-4 text-lg border border-black rounded-xl focus:ring-4 focus:ring-black focus:border-black bg-white text-black font-normal"
              placeholder="Juan Pérez"
            />
          </div>

          <div>
            <label htmlFor="email" className="block text-xl font-bold text-black mb-2">
              Email
            </label>
            <input
              id="email"
              name="email"
              type="email"
              value={formData.email}
              onChange={handleChange}
              required
              className="w-full px-6 py-4 text-lg border border-black rounded-xl focus:ring-4 focus:ring-black focus:border-black bg-white text-black font-normal"
              placeholder="tu@email.com"
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-xl font-bold text-black mb-2">
              Contraseña
            </label>
            <input
              id="password"
              name="password"
              type="password"
              value={formData.password}
              onChange={handleChange}
              required
              className="w-full px-6 py-4 text-lg border border-black rounded-xl focus:ring-4 focus:ring-black focus:border-black bg-white text-black font-normal"
              placeholder="••••••••"
            />
          </div>

          <div>
            <label htmlFor="phone" className="block text-xl font-bold text-black mb-2">
              Teléfono (Opcional)
            </label>
            <input
              id="phone"
              name="phone"
              type="tel"
              value={formData.phone}
              onChange={handleChange}
              className="w-full px-6 py-4 text-lg border border-black rounded-xl focus:ring-4 focus:ring-black focus:border-black bg-white text-black font-normal"
              placeholder="+57 300 123 4567"
            />
          </div>

          <div>
            <label htmlFor="location" className="block text-xl font-bold text-black mb-2">
              Ubicación (Opcional)
            </label>
            <input
              id="location"
              name="location"
              type="text"
              value={formData.location}
              onChange={handleChange}
              className="w-full px-6 py-4 text-lg border border-black rounded-xl focus:ring-4 focus:ring-black focus:border-black bg-white text-black font-normal"
              placeholder="Cundinamarca, Colombia"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-black text-white py-6 rounded-xl text-2xl font-bold hover:bg-gray-800 transition-all shadow-2xl disabled:opacity-50 disabled:cursor-not-allowed border border-black"
          >
            {loading ? 'Registrando...' : 'Crear Cuenta'}
          </button>
        </form>

        <p className="mt-8 text-center text-xl text-black font-semibold">
          ¿Ya tienes cuenta?{' '}
          <Link to="/login" className="text-black font-bold hover:underline underline">
            Inicia sesión aquí
          </Link>
        </p>
      </div>
    </div>
  )
}
