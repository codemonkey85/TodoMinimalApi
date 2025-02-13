import type { Route } from "./+types/home";
import WeatherForecast from "~/WeatherForecast";

// eslint-disable-next-line no-empty-pattern, react-refresh/only-export-components
export function meta({ }: Route.MetaArgs) {
    return [
        {
            title: "Weather",
            name: "description", content: "The local weather."
        }
    ]
}

export default function Home() {
    return <WeatherForecast />
}