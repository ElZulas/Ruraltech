import { Link } from 'react-router-dom'
import { CheckCircle, Smartphone, Scale, Syringe, FileText, ShoppingBag, BarChart3, Shield, Users, Clock, ArrowRight, Phone, Mail as MailIcon } from 'lucide-react'
import { useState } from 'react'

export default function LandingPage() {
  const [isAnimating, setIsAnimating] = useState(false)

  const handleLinkClick = () => {
    setIsAnimating(true)
    setTimeout(() => setIsAnimating(false), 600)
  }

  return (
    <div className="min-h-screen">
      <div className="container mx-auto px-4 py-8">
        {/* Header Formal - Estructura Superior */}
        <div className="bg-white rounded-2xl shadow-2xl p-10 mb-8 border border-black">
          <div className="flex items-center justify-center mb-6">
            <div className="inline-flex items-center justify-center w-32 h-32 bg-white rounded-3xl shadow-2xl border border-black overflow-hidden p-2">
              <img src={`${import.meta.env.BASE_URL}images/logo_front.jpeg`} alt="Cownect Logo" className="w-full h-full object-contain max-w-full max-h-full" />
            </div>
          </div>
          <div className="text-center">
            <h1 className="text-6xl font-bold text-black mb-4">Cownect</h1>
            <div className="w-32 h-1 bg-black mx-auto mb-4"></div>
            <p className="text-3xl text-black mb-4 font-semibold">Sistema de Gestión Ganadera</p>
            <p className="text-xl text-black max-w-3xl mx-auto font-normal leading-relaxed mb-8">
              Plataforma profesional para la gestión integral de su explotación ganadera.
              Registre animales, controle vacunaciones, gestione pesos y optimice su producción.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center items-center mt-6">
              <Link
                to="/register"
                onClick={handleLinkClick}
                className={`bg-black text-white px-8 py-4 rounded-xl text-xl font-bold hover:bg-gray-800 transition-all shadow-xl flex items-center gap-2 border border-black ${
                  isAnimating ? 'animate-pulse scale-105' : ''
                }`}
              >
                Registrarse Ahora
                <ArrowRight className={`h-6 w-6 transition-transform ${isAnimating ? 'translate-x-2' : ''}`} />
              </Link>
              <Link
                to="/login"
                onClick={handleLinkClick}
                className={`bg-white text-black px-8 py-4 rounded-xl text-xl font-bold hover:bg-gray-100 transition-all shadow-xl border border-black ${
                  isAnimating ? 'animate-pulse scale-105' : ''
                }`}
              >
                Ya tengo cuenta
              </Link>
            </div>
          </div>
        </div>

        {/* Sección: Funcionalidades y Beneficios - Dos Columnas */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
          {/* Columna Izquierda: Funcionalidades del Sistema */}
          <div className="bg-white rounded-2xl shadow-2xl p-10 border border-black">
            <h2 className="text-3xl font-bold text-black mb-8 text-center border-b-2 border-black pb-4">
              Funcionalidades del Sistema
            </h2>
            <div className="space-y-6">
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center overflow-hidden p-2">
                  <img src={`${import.meta.env.BASE_URL}images/logo_front.jpeg`} alt="Gestión Completa" className="w-full h-full object-contain" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Gestión Integral</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Registre y administre todo su inventario ganadero desde una única plataforma centralizada</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <CheckCircle className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Sistema de Alertas</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Notificaciones automáticas para el control de vacunaciones y tratamientos veterinarios</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <Smartphone className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Acceso Multiplataforma</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Disponible en dispositivos móviles y de escritorio para acceso en cualquier momento y lugar</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <Scale className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Control de Peso</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Registro y seguimiento del peso de sus animales con gráficos de evolución histórica</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <Syringe className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Gestión de Vacunas</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Registro completo del historial de vacunaciones con fechas de aplicación y próximas dosis</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <FileText className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Historial de Tratamientos</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Documentación completa de tratamientos veterinarios y condiciones médicas de cada animal</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <ShoppingBag className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Marketplace Ganadero</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Plataforma de comercio para compra y venta de productos y servicios relacionados con la ganadería</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <BarChart3 className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Reportes y Estadísticas</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Dashboard con métricas e indicadores clave para la toma de decisiones informadas</p>
                </div>
              </div>
            </div>
          </div>

          {/* Columna Derecha: Beneficios del Sistema */}
          <div className="bg-white rounded-2xl shadow-2xl p-10 border border-black">
            <h2 className="text-3xl font-bold text-black mb-8 text-center border-b-2 border-black pb-4">
              Beneficios del Sistema
            </h2>
            <div className="space-y-6">
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <Shield className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Seguridad de Datos</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Sus datos están protegidos con encriptación y respaldos automáticos</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <Clock className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Ahorro de Tiempo</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Automatice procesos y reduzca el tiempo de gestión administrativa</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <Users className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Gestión Colaborativa</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Trabaje en equipo con acceso controlado para múltiples usuarios</p>
                </div>
              </div>
              
              <div className="flex items-start gap-4">
                <div className="bg-black rounded-xl shadow-xl flex-shrink-0 w-16 h-16 flex items-center justify-center">
                  <BarChart3 className="h-10 w-10 text-white" />
                </div>
                <div>
                  <h3 className="text-2xl font-bold text-black mb-2">Análisis de Datos</h3>
                  <p className="text-lg text-gray-800 leading-relaxed font-normal">Obtenga insights valiosos para mejorar la productividad de su explotación</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Sección: Cómo Funciona */}
        <div className="bg-white rounded-2xl shadow-2xl p-10 mb-8 border border-black">
          <h2 className="text-3xl font-bold text-black mb-8 text-center border-b-2 border-black pb-4">
            ¿Cómo Funciona?
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto">
            <div className="text-center">
              <div className="bg-black rounded-xl shadow-xl w-20 h-20 flex items-center justify-center mx-auto mb-4">
                <span className="text-3xl font-bold text-white">1</span>
              </div>
              <h3 className="text-2xl font-bold text-black mb-3">Regístrese</h3>
              <p className="text-lg text-gray-800 font-normal leading-relaxed">Complete el formulario con sus datos personales para crear su cuenta de acceso</p>
            </div>
            <div className="text-center">
              <div className="bg-black rounded-xl shadow-xl w-20 h-20 flex items-center justify-center mx-auto mb-4">
                <span className="text-3xl font-bold text-white">2</span>
              </div>
              <h3 className="text-2xl font-bold text-black mb-3">Configure su Explotación</h3>
              <p className="text-lg text-gray-800 font-normal leading-relaxed">Registre sus animales y configure los parámetros de su explotación ganadera</p>
            </div>
            <div className="text-center">
              <div className="bg-black rounded-xl shadow-xl w-20 h-20 flex items-center justify-center mx-auto mb-4">
                <span className="text-3xl font-bold text-white">3</span>
              </div>
              <h3 className="text-2xl font-bold text-black mb-3">Gestione y Optimice</h3>
              <p className="text-lg text-gray-800 font-normal leading-relaxed">Utilice las herramientas del sistema para gestionar y optimizar su producción</p>
            </div>
          </div>
        </div>

        {/* Footer */}
        <footer className="bg-white rounded-2xl shadow-2xl p-8 border border-black">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-6">
            <div>
              <h3 className="text-2xl font-bold text-black mb-4">Cownect</h3>
              <p className="text-lg text-gray-800 font-normal">
                Sistema profesional de gestión ganadera para optimizar su explotación
              </p>
            </div>
            <div>
              <h4 className="text-xl font-bold text-black mb-4">Enlaces Rápidos</h4>
              <ul className="space-y-2">
                <li><a href="/login" className="text-lg text-gray-800 font-normal hover:text-black">Iniciar Sesión</a></li>
                <li><a href="/register" className="text-lg text-gray-800 font-normal hover:text-black">Registrarse</a></li>
                <li><a href="/app/dashboard" className="text-lg text-gray-800 font-normal hover:text-black">Acceder al Sistema</a></li>
              </ul>
            </div>
            <div>
              <h4 className="text-xl font-bold text-black mb-4">Contacto</h4>
              <ul className="space-y-2">
                <li className="flex items-center gap-2 text-lg text-gray-800 font-normal">
                  <MailIcon className="h-5 w-5" />
                  soporte@cownect.com
                </li>
                <li className="flex items-center gap-2 text-lg text-gray-800 font-normal">
                  <Phone className="h-5 w-5" />
                  +57 300 123 4567
                </li>
              </ul>
            </div>
          </div>
          <div className="border-t-2 border-black pt-6 text-center">
            <p className="text-lg text-gray-800 font-normal">
              © 2024 Cownect. Todos los derechos reservados.
            </p>
          </div>
        </footer>
      </div>
    </div>
  )
}
