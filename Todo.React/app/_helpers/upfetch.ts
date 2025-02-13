import { up } from 'up-fetch'

export const upfetch = up(fetch, () => ({
    baseUrl: __API_URL__
}))

export default upfetch