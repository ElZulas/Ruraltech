import { Outlet, Link, useLocation } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { Home, ShoppingBag, AlertTriangle, LogOut } from 'lucide-react'

export default function Layout() {
  const { logout, user } = useAuth()
  const location = useLocation()

  const isActive = (path: string) => location.pathname === path

  return (
    <div className="min-h-screen">
      {/* Header */}
      <header className="bg-black/95 backdrop-blur-md text-white shadow-2xl border-b border-green-500/20">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="bg-white/20 p-1 rounded-lg overflow-hidden w-8 h-8 flex items-center justify-center">
                <img src={`${import.meta.env.BASE_URL}images/logo_front.jpeg`} alt="Cownect" className="h-6 w-6 object-contain max-w-full max-h-full" />
              </div>
              <div>
                <h1 className="text-2xl font-bold">Cownect</h1>
                <p className="text-sm text-white/90">Sistema de Gestión Ganadera</p>
              </div>
            </div>
            <div className="flex items-center gap-4">
              <span className="text-sm">{user?.fullName}</span>
              <button
                onClick={logout}
                className="p-2 hover:bg-white/20 rounded-lg transition-colors"
                title="Cerrar sesión"
              >
                <LogOut className="h-5 w-5" />
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="container mx-auto px-4 py-6 pb-24">
        <Outlet />
      </main>

      {/* Bottom Navigation */}
      <nav className="fixed bottom-0 left-0 right-0 bg-white/98 backdrop-blur-md border-t border-black/10 shadow-2xl">
        <div className="container mx-auto px-4 py-3">
          <div className="flex justify-around items-center">
            <Link
              to="/app/dashboard"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors ${
                isActive('/app/dashboard') ? 'bg-black text-white shadow-lg border-t border-green-500/50' : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
              }`}
            >
              <Home className="h-5 w-5" />
              <span className="text-xs font-medium">Inicio</span>
            </Link>
            <Link
              to="/app/animals"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors ${
                isActive('/app/animals') ? 'bg-black text-white shadow-lg border-t border-green-500/50' : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
              }`}
            >
              <img src={`${import.meta.env.BASE_URL}images/logo_front.jpeg`} alt="Animales" className="h-5 w-5 object-contain max-w-full max-h-full" />
              <span className="text-xs font-medium">Animales</span>
            </Link>
            <Link
              to="/app/marketplace"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors ${
                isActive('/app/marketplace') ? 'bg-black text-white shadow-lg border-t border-green-500/50' : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
              }`}
            >
              <ShoppingBag className="h-5 w-5" />
              <span className="text-xs font-medium">Marketplace</span>
            </Link>
            <Link
              to="/app/alerts"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors relative ${
                isActive('/app/alerts') ? 'bg-black text-white shadow-lg border-t border-green-500/50' : 'text-gray-600 hover:bg-gray-100 hover:text-gray-900'
              }`}
            >
              <AlertTriangle className="h-5 w-5" />
              <span className="text-xs font-medium">Alertas</span>
            </Link>
          </div>
        </div>
      </nav>
    </div>
  )
}
