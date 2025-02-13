import { z } from 'zod'

export interface IWeatherForecast {
    id: number,
    date: string,
    temperatureC: number,
    summary: string,
    temperatureF: number,
}

export const WeatherForecastSchema = z.object({
    id: z.number().nonnegative(),
    temperatureC: z.number(),
    temperatureF: z.number(),
    summary: z.string(),
    date: z.string()
})

export const WeatherForecastListSchema = z.array(WeatherForecastSchema)
