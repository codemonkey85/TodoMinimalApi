import { type IWeatherForecast, WeatherForecastListSchema } from './models'
import { up } from 'up-fetch'
import { useQuery } from '@tanstack/react-query'

function WeatherForecast() {
  const upfetch = up(fetch, () => ({
    baseUrl: __API_URL__
  }))

  const { isPending, error, data } = useQuery<IWeatherForecast[]>({
    queryKey: ['weatherForecasts'],
    queryFn: async () => {
      const data = await upfetch('/api/weatherforecast', { schema: WeatherForecastListSchema })
      return data
    }
  })

  if (isPending) return 'Loading...'

  if (error) return 'An error has occurred: ' + error.message
  
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
        {data.map((forecast) => (
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

export default WeatherForecast
