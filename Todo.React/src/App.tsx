import { useEffect, useState } from 'react'
import './App.css'
import { IWeatherForecast, WeatherForecastListSchema } from './models'
import { up, isResponseError, isValidationError } from 'up-fetch'

function App() {
  const upfetch = up(fetch, () => ({
    baseUrl: __API_URL__
  }))
  const [forecasts, setForecasts] = useState<IWeatherForecast[]>([])
  
  useEffect(() => {
    const fetchForecasts = async () => {
      try {
        const data = await upfetch('/api/weatherforecast',
          { schema: WeatherForecastListSchema }
        )
        setForecasts(data)
        console.log(data)
      } catch (error) {
        if (isResponseError(error)) {
          console.log(error.status)
        }
        if (isValidationError(error)) {
          console.error(error.issues)
        }
        else {
          console.error(error)
        }
      }
    }

    fetchForecasts()
  }, [])
  return (
    <table>
      <thead>
        <tr>
            <th>Date</th>
            <th aria-label="Temperature in Celsius">Temp. (C)</th>
            <th aria-label="Temperature in Farenheit">Temp. (F)</th>
            <th>Summary</th>
        </tr>
      </thead>
      <tbody>
        {forecasts.map((forecast) => (
          <tr key={forecast.id}>
            <td>{new Date(forecast.date).toLocaleString()}</td>
            <td>{forecast.temperatureC}</td>
            <td>{forecast.temperatureF}</td>
            <td>{forecast.summary}</td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}

export default App
