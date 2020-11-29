import React, { useEffect, useState } from 'react'
import { useRouteMatch } from 'react-router-dom'
import LayoutSection from '../../components/Layout/LayoutSection'
import { Button } from '../../shared'
import { shallowEqual, useDispatch, useSelector } from 'react-redux'
import { PageLoader, PageMessage } from '../../components/Page'
import {
  addComment,
  loadTasks,
  loadWorkers,
  setTask, setTaskDetails, setTasks,
} from '../../reducers/data'
import Table from '../../components/Table'
import { createPathURL, getFormattedDate } from '../../shared/helpers'
import { DatumPriority, DatumStatus } from '../../components/Datum'
import TaskDetails from '../../components/TaskDetails'
import { useToasts } from 'react-toast-notifications'
import {
  fetchTasks,
  finishTask,
  sendComment,
  uploadVoice,
} from '../../services/task'
import { setRedirect } from '../../reducers/layout'
import RecordButton from '../../components/TaskModal/RecordButton'
import PlayAudio from '../../components/TaskDetails/PlayAudio'

const Outbox = function () {
  const match = useRouteMatch()
  const data = useSelector(({ data }) => data.outbox, shallowEqual)
  const workers = useSelector(({ data }) => data.workers, shallowEqual)
  const audioGuid = useSelector(({ data }) => data.details && data.details.audioGuid)
  const dispatch = useDispatch()
  const taskId = match.params.task

  const userId = useSelector(({ user }) => user.id)
  const [ isRecognizing, setRecognizing ] = useState(false)
  const { addToast } = useToasts()

  const handleVoiceSend = async function (audioBlob) {
    setRecognizing(true)
    const data = await uploadVoice(userId, audioBlob)
    setRecognizing(false)
    if (data.error) {
      addToast(data.error, {
        appearance: 'error',
        autoDismissTimeout: 3000,
        autoDismiss: true
      })
    } else {
      if (data.action === 'task') {
        dispatch(setTask(data))
        dispatch(setRedirect(`${match.url}/@new-task`))
      } else if (data.action === 'comment'){
        const comment = await sendComment(data)
        dispatch(addComment(comment))
      } else if (data.action === 'finish') {
        const task = await finishTask(data)
        const tasks = await fetchTasks(userId, match.params.page)
        dispatch(setTaskDetails(task))
        dispatch(setTasks(match.params.page, tasks))
      }
    }
  }
  
  useEffect(function () {
    dispatch(loadTasks('outbox'))
    dispatch(loadWorkers())
  }, [dispatch])
  
  
  return <>
    <LayoutSection header={(<>
      <strong>Исходящие</strong>
      <RecordButton
        small
        isRecognizing={isRecognizing}
        isDisabled={isRecognizing}
        onSend={handleVoiceSend}
      />
    </>)}>
      {!data || !workers
        ? <PageLoader />
        : data.length > 0 
          ? <Table
            labels={[
              <strong style={{fontSize: '14px'}}>⥮</strong>,
              '№',
              'Название',
              'Объект',
              'Выполнить до',
              'Исполнитель',
              'Статус'
            ]}
            sizes={[
              '30px',
              '50px',
              'auto',
              'auto',
              '150px',
              '150px',
              '110px',
            ]}
            data={data.map(item => ({
              to: createPathURL(match.path, {...match.params, task: item.id}),
              selected: match.params.task === item.id.toString(),
              datum: {
                priority: <DatumPriority priority={item.priority} />,
                id: item.id,
                name: <strong>{item.name}</strong>,
                taskObject: item.taskObject,
                plannedAt: getFormattedDate(item.plannedAt),
                executor: workers.find(x => x.id === item.executorId).name,
                status: item.isFinished
                  ? <DatumStatus className={'-success'}>Завершено</DatumStatus>
                  : <DatumStatus>Активно</DatumStatus>
              }
            }))}
          />
        : <PageMessage>
          Задачи отсутствуют
        </PageMessage>
      }
    </LayoutSection>
    <LayoutSection 
      aside 
      header={taskId 
        ? <>
          <span>Задача №{taskId}</span>
          {audioGuid && <PlayAudio guid={audioGuid} />}
        </> 
        : null
      }
    >
      <TaskDetails />
    </LayoutSection>
  </>
}

export default Outbox