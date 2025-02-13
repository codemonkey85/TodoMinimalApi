import { type IWeatherForecast } from './models'

type FuncParams = {
  forecasts: IWeatherForecast[]
}

export default function WeatherForecast(params: FuncParams) {
  const { forecasts } = params;
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
