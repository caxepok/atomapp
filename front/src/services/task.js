import { DEV_API } from '../shared/consts'

export async function uploadVoice (userId, data) {
  const formData = new FormData()
  formData.append('filename', data)

  const response = await fetch(`${DEV_API}/record/upload?userId=${userId}&isOpus=true`, {
    method: 'POST',
    headers: {
      Accept: 'application/json',
    },
    body: formData
  })
  
  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function fetchTemplate (userId, number) {
  const response = await fetch(`${DEV_API}/record/upload/template/${number}?userId=${userId}&isOpus=false`, {
    method: 'POST',
  })

  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function fetchTaskWorkers (userId) {
  const response = await fetch(`${DEV_API}/workplace/worker/subordinates?userId=${userId}`, {
    method: 'GET',
  })

  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function saveTask (userId, task) {
  const response = await fetch(`${DEV_API}/task?userId=${userId}`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(task)
  })

  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function fetchTasks (userId, page) {
  const response = await fetch(`${DEV_API}/task/${page}?userId=${userId}`, { method: 'GET'})
  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function fetchTaskDetails (userId, taskId) {
  const response = await fetch(`${DEV_API}/task/${taskId}?userId=${userId}`, { method: 'GET'})
  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function sendComment (userId, taskId, comment) {
  const response = await fetch(`${DEV_API}/task/${taskId}/comment`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      taskId,
      creatorId: userId,
      comment
    })
  })

  if (response.status === 200) {
    return await response.json()
  } else {
    throw Error()
  }
}

export async function finishTask (userId, taskId, comment) {
  const response = await fetch(`${DEV_API}/task/${taskId}/finish`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      taskId,
      creatorId: userId,
      comment
    })
  })

  if (response.status === 204) {
    return await fetchTaskDetails(userId, taskId)
  } else {
    throw Error()
  }
}