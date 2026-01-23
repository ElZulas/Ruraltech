import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import api from '../services/api'
import { Beef, Plus, AlertTriangle, TrendingUp } from 'lucide-react'

interface DashboardStats {
  totalAnimals: number
  totalAlerts: number
  upcomingVaccines: number
}

export default function DashboardPage() {
  const [stats, setStats] = useState<DashboardStats>({
    totalAnimals: 0,
    totalAlerts: 0,
    upcomingVaccines: 0,
  })
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadStats()
  }, [])

  const loadStats = async () => {
    try {
      const [animalsRes, alertsRes] = await Promise.all([
        api.get('/animals'),
        api.get('/alerts'),
      ])

      const alerts = alertsRes.data
      setStats({
        totalAnimals: animalsRes.data.length,
        totalAlerts: alerts.length,
        upcomingVaccines: alerts.filter((a: any) => a.daysLeft <= 7).length,
      })
    } catch (error) {
      console.error('Error loading stats:', error)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return <div className="flex items-center justify-center min-h-[60vh]">Cargando...</div>
  }

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-3xl font-bold text-gray-900 mb-2">Dashboard</h2>
        <p className="text-gray-600">Resumen de tu ganado</p>
      </div>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <div className="bg-gradient-to-br from-blue-500 to-blue-600 text-white rounded-2xl p-6 shadow-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-blue-100 text-sm mb-1">Total Animales</p>
              <p className="text-3xl font-bold">{stats.totalAnimals}</p>
            </div>
            <Beef className="h-12 w-12 opacity-80" />
          </div>
        </div>

        <div className="bg-gradient-to-br from-green-500 to-green-600 text-white rounded-2xl p-6 shadow-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-green-100 text-sm mb-1">Alertas Activas</p>
              <p className="text-3xl font-bold">{stats.totalAlerts}</p>
            </div>
            <AlertTriangle className="h-12 w-12 opacity-80" />
          </div>
        </div>

        <div className="bg-gradient-to-br from-orange-500 to-orange-600 text-white rounded-2xl p-6 shadow-lg">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-orange-100 text-sm mb-1">Vacunas Urgentes</p>
              <p className="text-3xl font-bold">{stats.upcomingVaccines}</p>
            </div>
            <TrendingUp className="h-12 w-12 opacity-80" />
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="bg-white rounded-2xl shadow-lg p-6">
        <h3 className="text-xl font-bold text-gray-900 mb-4">Acciones Rápidas</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <Link
            to="/app/animals"
            className="flex items-center gap-4 p-4 border-2 border-gray-200 rounded-xl hover:border-blue-500 hover:bg-blue-50 transition-all"
          >
            <div className="bg-blue-100 p-3 rounded-lg">
              <Beef className="h-6 w-6 text-blue-600" />
            </div>
            <div>
              <h4 className="font-semibold text-gray-900">Ver Animales</h4>
              <p className="text-sm text-gray-600">Gestiona tu ganado</p>
            </div>
          </Link>

          <Link
            to="/app/animals?new=true"
            className="flex items-center gap-4 p-4 border-2 border-gray-200 rounded-xl hover:border-green-500 hover:bg-green-50 transition-all"
          >
            <div className="bg-green-100 p-3 rounded-lg">
              <Plus className="h-6 w-6 text-green-600" />
            </div>
            <div>
              <h4 className="font-semibold text-gray-900">Agregar Animal</h4>
              <p className="text-sm text-gray-600">Registra un nuevo animal</p>
            </div>
          </Link>

          <Link
            to="/app/marketplace"
            className="flex items-center gap-4 p-4 border-2 border-gray-200 rounded-xl hover:border-orange-500 hover:bg-orange-50 transition-all"
          >
            <div className="bg-orange-100 p-3 rounded-lg">
              <TrendingUp className="h-6 w-6 text-orange-600" />
            </div>
            <div>
              <h4 className="font-semibold text-gray-900">Marketplace</h4>
              <p className="text-sm text-gray-600">Explora productos</p>
            </div>
          </Link>

          <Link
            to="/app/alerts"
            className="flex items-center gap-4 p-4 border-2 border-gray-200 rounded-xl hover:border-red-500 hover:bg-red-50 transition-all"
          >
            <div className="bg-red-100 p-3 rounded-lg">
              <AlertTriangle className="h-6 w-6 text-red-600" />
            </div>
            <div>
              <h4 className="font-semibold text-gray-900">Ver Alertas</h4>
              <p className="text-sm text-gray-600">Vacunas próximas a vencer</p>
            </div>
          </Link>
        </div>
      </div>
    </div>
  )
}
