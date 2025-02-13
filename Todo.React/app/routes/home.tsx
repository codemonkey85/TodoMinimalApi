import type { Route } from "./+types/home";
import WeatherForecast from "~/WeatherForecast";
import { type IWeatherForecast, WeatherForecastListSchema } from "~/models";
import { upfetch } from '~/_helpers'

// eslint-disable-next-line no-empty-pattern, react-refresh/only-export-components
export function meta({ }: Route.MetaArgs) {
    return [
        {
            title: "Weather",
            name: "description", content: "The local weather."
        }
    ]
}

export async function clientLoader({
    params,
}: Route.ClientLoaderArgs) {
    const data = await upfetch('/api/weatherforecast', {
        schema: WeatherForecastListSchema
    })
    return data
}

export function HydrateFallback() {
  return <div>Loading...</div>;
}

export default function Home({
    loaderData
}: Route.ComponentProps) {
    const forecasts: IWeatherForecast[] = loaderData
    return <WeatherForecast forecasts={forecasts} />
}