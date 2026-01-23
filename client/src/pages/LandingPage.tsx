import { useState } from 'react'
import { Beef, Download, CheckCircle, Mail, Smartphone } from 'lucide-react'
import api from '../services/api'

export default function LandingPage() {
  const [formData, setFormData] = useState({
    email: '',
    fullName: '',
    dateOfBirth: '',
    phone: '',
    location: '',
  })
  const [loading, setLoading] = useState(false)
  const [success, setSuccess] = useState(false)
  const [error, setError] = useState('')

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      // Generar contraseña temporal
      const tempPassword = Math.random().toString(36).slice(-8) + Math.random().toString(36).slice(-8).toUpperCase() + '123!'
      
      // Validar fecha de nacimiento (mayor de 18 años)
      if (!formData.dateOfBirth) {
        setError('La fecha de nacimiento es requerida')
        setLoading(false)
        return
      }

      const birthDate = new Date(formData.dateOfBirth)
      const today = new Date()
      const age = today.getFullYear() - birthDate.getFullYear()
      const monthDiff = today.getMonth() - birthDate.getMonth()
      const actualAge = monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate()) ? age - 1 : age

      if (actualAge < 18) {
        setError('Debes ser mayor de 18 años para registrarte como Propietario Legal')
        setLoading(false)
        return
      }

      // Registrar usuario
      const response = await api.post('/auth/register', {
        email: formData.email,
        password: tempPassword,
        fullName: formData.fullName,
        dateOfBirth: formData.dateOfBirth,
        phone: formData.phone || undefined,
        location: formData.location || undefined,
      })

      // Enviar email de confirmación
      try {
        await api.post('/email/send-confirmation', {
          email: formData.email,
          fullName: formData.fullName,
          tempPassword: tempPassword,
        })
      } catch (emailError) {
        // Si falla el email, continuar de todas formas
        console.log('Email no enviado (modo desarrollo):', emailError)
      }

      setSuccess(true)
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al registrar. Intenta de nuevo.')
    } finally {
      setLoading(false)
    }
  }

  const handleDownload = () => {
    // Redirigir a la app PWA
    window.location.href = '/app/dashboard'
  }

  const handleDownloadPC = () => {
    // Descargar instalador de Windows
    window.open('/api/download/pc', '_blank')
  }

  const handleDownloadAndroid = () => {
    // Descargar APK de Android
    window.open('/api/download/android', '_blank')
  }

  if (success) {
    return (
      <div className="min-h-screen bg-gradient-to-br from-blue-50 via-green-50 to-orange-50 flex items-center justify-center px-4">
        <div className="max-w-2xl w-full bg-white rounded-2xl shadow-xl p-8 text-center">
          <div className="inline-flex items-center justify-center w-20 h-20 bg-green-100 rounded-full mb-6">
            <CheckCircle className="h-10 w-10 text-green-600" />
          </div>
          <h2 className="text-3xl font-bold text-gray-900 mb-4">¡Registro Exitoso!</h2>
          <p className="text-gray-600 mb-2">
            Hemos enviado un correo de confirmación a <strong>{formData.email}</strong>
          </p>
          <p className="text-sm text-gray-500 mb-8">
            Revisa tu bandeja de entrada para obtener tus credenciales de acceso.
          </p>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
            <button
              onClick={handleDownloadPC}
              className="bg-gradient-to-r from-blue-600 to-blue-700 text-white py-6 rounded-xl font-semibold hover:from-blue-700 hover:to-blue-800 transition-all flex flex-col items-center justify-center gap-2 shadow-lg"
            >
              <Download className="h-8 w-8" />
              <span className="text-lg">Descargar para PC</span>
              <span className="text-sm opacity-90">Windows Installer</span>
            </button>

            <button
              onClick={handleDownloadAndroid}
              className="bg-gradient-to-r from-green-600 to-green-700 text-white py-6 rounded-xl font-semibold hover:from-green-700 hover:to-green-800 transition-all flex flex-col items-center justify-center gap-2 shadow-lg"
            >
              <Smartphone className="h-8 w-8" />
              <span className="text-lg">Descargar para Android</span>
              <span className="text-sm opacity-90">APK Installer</span>
            </button>
          </div>

          <p className="text-xs text-gray-500 mt-6">
            O accede desde tu navegador en: <a href="/app/dashboard" className="text-blue-600 hover:underline">ruraltech.app/app</a>
          </p>
        </div>
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-green-50 to-orange-50">
      {/* Hero Section */}
      <div className="container mx-auto px-4 py-16">
        <div className="text-center mb-12">
          <div className="inline-flex items-center justify-center w-24 h-24 bg-gradient-to-r from-blue-600 to-green-600 rounded-3xl mb-6 shadow-lg">
            <Beef className="h-12 w-12 text-white" />
          </div>
          <h1 className="text-5xl font-bold text-gray-900 mb-4">RuralTech</h1>
          <p className="text-2xl text-gray-700 mb-2">Gestión Inteligente de Ganado</p>
          <p className="text-lg text-gray-600 max-w-2xl mx-auto">
            Simplifica el manejo de tu ganado con herramientas diseñadas para el campo.
            Registra animales, controla vacunas, gestiona peso y mucho más.
          </p>
        </div>

        {/* Features */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-12 max-w-4xl mx-auto">
          <div className="bg-white rounded-2xl p-6 shadow-lg">
            <div className="bg-blue-100 p-4 rounded-xl w-fit mb-4">
              <Beef className="h-8 w-8 text-blue-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">Gestión Completa</h3>
            <p className="text-gray-600">Registra y gestiona todos tus animales en un solo lugar</p>
          </div>
          <div className="bg-white rounded-2xl p-6 shadow-lg">
            <div className="bg-green-100 p-4 rounded-xl w-fit mb-4">
              <CheckCircle className="h-8 w-8 text-green-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">Alertas Automáticas</h3>
            <p className="text-gray-600">Nunca olvides una vacuna con nuestro sistema de alertas</p>
          </div>
          <div className="bg-white rounded-2xl p-6 shadow-lg">
            <div className="bg-orange-100 p-4 rounded-xl w-fit mb-4">
              <Smartphone className="h-8 w-8 text-orange-600" />
            </div>
            <h3 className="text-xl font-bold text-gray-900 mb-2">App Móvil</h3>
            <p className="text-gray-600">Accede desde cualquier dispositivo, en cualquier momento</p>
          </div>
        </div>

        {/* Registration Form */}
        <div className="max-w-md mx-auto bg-white rounded-2xl shadow-xl p-8">
          <h2 className="text-2xl font-bold text-gray-900 mb-6 text-center">
            Regístrate Gratis
          </h2>

          {error && (
            <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-700 rounded-lg text-sm">
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label htmlFor="fullName" className="block text-sm font-medium text-gray-700 mb-1">
                Nombre Completo
              </label>
              <input
                id="fullName"
                name="fullName"
                type="text"
                value={formData.fullName}
                onChange={handleChange}
                required
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="Juan Pérez"
              />
            </div>

            <div>
              <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-1">
                Email
              </label>
              <input
                id="email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                required
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="tu@email.com"
              />
            </div>

            <div>
              <label htmlFor="dateOfBirth" className="block text-sm font-medium text-gray-700 mb-1">
                Fecha de Nacimiento
              </label>
              <input
                id="dateOfBirth"
                name="dateOfBirth"
                type="date"
                value={formData.dateOfBirth}
                onChange={handleChange}
                required
                max={new Date(new Date().setFullYear(new Date().getFullYear() - 18)).toISOString().split('T')[0]}
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
              <p className="text-xs text-gray-500 mt-1">Debes ser mayor de 18 años</p>
            </div>

            <div>
              <label htmlFor="phone" className="block text-sm font-medium text-gray-700 mb-1">
                Teléfono (Opcional)
              </label>
              <input
                id="phone"
                name="phone"
                type="tel"
                value={formData.phone}
                onChange={handleChange}
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="+52 123 456 7890"
              />
              <p className="text-xs text-gray-500 mt-1">Formato internacional (ej. +52...)</p>
            </div>

            <div>
              <label htmlFor="location" className="block text-sm font-medium text-gray-700 mb-1">
                Ubicación (Opcional)
              </label>
              <input
                id="location"
                name="location"
                type="text"
                value={formData.location}
                onChange={handleChange}
                className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="Cundinamarca, Colombia"
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="w-full bg-gradient-to-r from-blue-600 to-green-600 text-white py-4 rounded-xl font-semibold hover:from-blue-700 hover:to-green-700 transition-all disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
                  Registrando...
                </>
              ) : (
                <>
                  <Mail className="h-5 w-5" />
                  Crear Cuenta Gratis
                </>
              )}
            </button>
          </form>

          <p className="text-xs text-gray-500 text-center mt-6">
            Al registrarte, aceptas nuestros términos y condiciones de uso.
          </p>
        </div>
      </div>
    </div>
  )
}
