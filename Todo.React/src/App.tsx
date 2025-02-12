import { useEffect, useState } from 'react'
import './App.css'
import { IWeatherForecast } from './models'
import { up, isResponseError } from 'up-fetch'

function App() {
  const upfetch = up(fetch, () => ({
    baseUrl: __API_URL__
  }))
  const [forecasts, setForecasts] = useState<IWeatherForecast[]>([])
  
  useEffect(() => {
    const fetchForecasts = async () => {
      try {
        const data = await upfetch('/api/weatherforecast')
        setForecasts(data)
        console.log(data)
      } catch (error) {
        if (isResponseError(error)) {
            console.log(error.status)
        }
      }
    }

    fetchForecasts()
  }, [])
  return (
    <ul>
      {forecasts.map((forecast) => (
        <li key={forecast.temperatureC}>
          {forecast.summary}
        </li>
      ))}
    </ul>
  )
}

export default App
