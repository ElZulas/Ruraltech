import { Outlet, Link, useLocation } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { Home, Beef, ShoppingBag, AlertTriangle, LogOut } from 'lucide-react'

export default function Layout() {
  const { logout, user } = useAuth()
  const location = useLocation()

  const isActive = (path: string) => location.pathname === path

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-green-50 to-orange-50">
      {/* Header */}
      <header className="bg-gradient-to-r from-blue-600 to-green-600 text-white shadow-lg">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <div className="bg-white/20 p-2 rounded-lg">
                <Beef className="h-6 w-6" />
              </div>
              <div>
                <h1 className="text-2xl font-bold">RuralTech</h1>
                <p className="text-sm text-white/90">Gestión inteligente de ganado</p>
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
      <nav className="fixed bottom-0 left-0 right-0 bg-white/95 backdrop-blur-md border-t border-gray-200 shadow-lg">
        <div className="container mx-auto px-4 py-3">
          <div className="flex justify-around items-center">
            <Link
              to="/app/dashboard"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors ${
                isActive('/app/dashboard') ? 'bg-blue-100 text-blue-600' : 'text-gray-600 hover:bg-gray-100'
              }`}
            >
              <Home className="h-5 w-5" />
              <span className="text-xs font-medium">Inicio</span>
            </Link>
            <Link
              to="/app/animals"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors ${
                isActive('/app/animals') ? 'bg-green-100 text-green-600' : 'text-gray-600 hover:bg-gray-100'
              }`}
            >
              <Beef className="h-5 w-5" />
              <span className="text-xs font-medium">Animales</span>
            </Link>
            <Link
              to="/app/marketplace"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors ${
                isActive('/app/marketplace') ? 'bg-orange-100 text-orange-600' : 'text-gray-600 hover:bg-gray-100'
              }`}
            >
              <ShoppingBag className="h-5 w-5" />
              <span className="text-xs font-medium">Marketplace</span>
            </Link>
            <Link
              to="/app/alerts"
              className={`flex flex-col items-center gap-1 px-4 py-2 rounded-xl transition-colors relative ${
                isActive('/app/alerts') ? 'bg-red-100 text-red-600' : 'text-gray-600 hover:bg-gray-100'
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
