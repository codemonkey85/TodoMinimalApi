import { z } from 'zod'

export interface IWeatherForecast {
    date: Date,
    temperatureC: number,
    summary: string,
    temperatureF: number,
}

export const WeatherForecastSchema = z.object({
    temperatureC: z.number(),
    summary: z.string(),
    date: z.date()
})

export default class WeatherForecast implements IWeatherForecast {
    readonly date: Date;
    readonly temperatureC: number;
    readonly summary: string;

    get temperatureF() {
        return 32 + (this.temperatureC / 0.5556)
    }

    constructor(date: Date, temperatureC: number, summary: string) {
        
        this.date = date;
        this.temperatureC = temperatureC;
        this.summary = summary;
    }
}