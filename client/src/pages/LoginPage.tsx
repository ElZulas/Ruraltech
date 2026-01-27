import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { Home } from 'lucide-react'

export default function LoginPage() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      await login(email, password)
      navigate('/app/dashboard')
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al iniciar sesión')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center px-4">
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
          <p className="text-xl text-black font-semibold">Inicio de Sesión</p>
          <p className="text-lg text-black mt-2 font-normal">Ingrese sus credenciales de acceso para continuar</p>
        </div>

        {error && (
          <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-700 rounded-lg text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-6">
          <div>
            <label htmlFor="email" className="block text-xl font-bold text-black mb-2">
              Email
            </label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="w-full px-6 py-4 text-lg border border-black rounded-xl focus:ring-4 focus:ring-black focus:border-black bg-white text-black font-normal"
              placeholder="••••••••"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full bg-black text-white py-6 rounded-xl text-2xl font-bold hover:bg-gray-800 transition-all shadow-2xl disabled:opacity-50 disabled:cursor-not-allowed border border-black"
          >
            {loading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
          </button>
        </form>

        <p className="mt-8 text-center text-xl text-black font-semibold">
          ¿No tienes cuenta?{' '}
          <Link to="/register" className="text-black font-bold hover:underline underline">
            Regístrate aquí
          </Link>
        </p>
      </div>
    </div>
  )
}
