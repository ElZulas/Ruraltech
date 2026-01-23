import { useEffect, useState } from 'react'
import api from '../services/api'
import { AlertTriangle, Calendar, Beef } from 'lucide-react'
import { Link } from 'react-router-dom'

interface Alert {
  animalId: number
  animalName: string
  vaccineName: string
  dueDate: string
  daysLeft: number
  isUrgent: boolean
}

export default function AlertsPage() {
  const [alerts, setAlerts] = useState<Alert[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadAlerts()
  }, [])

  const loadAlerts = async () => {
    try {
      const response = await api.get('/alerts')
      setAlerts(response.data)
    } catch (error) {
      console.error('Error loading alerts:', error)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return <div className="flex items-center justify-center min-h-[60vh]">Cargando...</div>
  }

  const urgentAlerts = alerts.filter((a) => a.isUrgent)
  const normalAlerts = alerts.filter((a) => !a.isUrgent)

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-3xl font-bold text-gray-900 mb-2">Alertas</h2>
        <p className="text-gray-600">Vacunas próximas a vencer</p>
      </div>

      {alerts.length === 0 ? (
        <div className="bg-white rounded-2xl shadow-lg p-12 text-center">
          <AlertTriangle className="h-16 w-16 text-green-400 mx-auto mb-4" />
          <h3 className="text-xl font-semibold text-gray-900 mb-2">No hay alertas activas</h3>
          <p className="text-gray-600">Todas las vacunas están al día</p>
        </div>
      ) : (
        <>
          {urgentAlerts.length > 0 && (
            <div>
              <h3 className="text-xl font-bold text-red-600 mb-4 flex items-center gap-2">
                <AlertTriangle className="h-5 w-5" />
                Alertas Urgentes ({urgentAlerts.length})
              </h3>
              <div className="space-y-3">
                {urgentAlerts.map((alert, index) => (
                  <Link
                    key={index}
                    to={`/app/animals/${alert.animalId}`}
                    className="block bg-red-50 border-2 border-red-200 rounded-2xl p-6 hover:bg-red-100 transition-colors"
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <div className="flex items-center gap-3 mb-2">
                          <div className="bg-red-100 p-2 rounded-lg">
                            <Beef className="h-5 w-5 text-red-600" />
                          </div>
                          <div>
                            <h4 className="font-bold text-gray-900">{alert.animalName}</h4>
                            <p className="text-sm text-gray-600">{alert.vaccineName}</p>
                          </div>
                        </div>
                        <div className="flex items-center gap-4 mt-4">
                          <div className="flex items-center gap-2 text-sm text-gray-600">
                            <Calendar className="h-4 w-4" />
                            <span>Vence: {new Date(alert.dueDate).toLocaleDateString('es-ES')}</span>
                          </div>
                          <span className="text-sm font-semibold text-red-600">
                            {alert.daysLeft === 0
                              ? 'Vence hoy'
                              : alert.daysLeft === 1
                              ? 'Vence mañana'
                              : `Faltan ${alert.daysLeft} días`}
                          </span>
                        </div>
                      </div>
                      <div className="bg-red-600 text-white px-4 py-2 rounded-xl font-bold">
                        {alert.daysLeft}
                      </div>
                    </div>
                  </Link>
                ))}
              </div>
            </div>
          )}

          {normalAlerts.length > 0 && (
            <div>
              <h3 className="text-xl font-bold text-orange-600 mb-4 flex items-center gap-2">
                <AlertTriangle className="h-5 w-5" />
                Próximas ({normalAlerts.length})
              </h3>
              <div className="space-y-3">
                {normalAlerts.map((alert, index) => (
                  <Link
                    key={index}
                    to={`/app/animals/${alert.animalId}`}
                    className="block bg-orange-50 border-2 border-orange-200 rounded-2xl p-6 hover:bg-orange-100 transition-colors"
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <div className="flex items-center gap-3 mb-2">
                          <div className="bg-orange-100 p-2 rounded-lg">
                            <Beef className="h-5 w-5 text-orange-600" />
                          </div>
                          <div>
                            <h4 className="font-bold text-gray-900">{alert.animalName}</h4>
                            <p className="text-sm text-gray-600">{alert.vaccineName}</p>
                          </div>
                        </div>
                        <div className="flex items-center gap-4 mt-4">
                          <div className="flex items-center gap-2 text-sm text-gray-600">
                            <Calendar className="h-4 w-4" />
                            <span>Vence: {new Date(alert.dueDate).toLocaleDateString('es-ES')}</span>
                          </div>
                          <span className="text-sm font-semibold text-orange-600">
                            Faltan {alert.daysLeft} días
                          </span>
                        </div>
                      </div>
                      <div className="bg-orange-600 text-white px-4 py-2 rounded-xl font-bold">
                        {alert.daysLeft}
                      </div>
                    </div>
                  </Link>
                ))}
              </div>
            </div>
          )}
        </>
      )}
    </div>
  )
}
