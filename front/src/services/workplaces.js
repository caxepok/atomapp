import { DEV_API } from '../shared/consts'

export async function fetchWorkplaces () {
  const response = await fetch(`${DEV_API}/workplace`)
  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function fetchUsers (id) {
  const response = await fetch(`${DEV_API}/workplace/${id}/worker`)
  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function fetchWorkers () {
  const response = await fetch(`${DEV_API}/workplace/worker`)
  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}