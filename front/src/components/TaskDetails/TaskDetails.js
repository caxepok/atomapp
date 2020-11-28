import React, { useEffect, useState } from 'react'
import { useRouteMatch } from 'react-router'
import { shallowEqual, useDispatch, useSelector } from 'react-redux'
import {
  addComment,
  loadTaskDetails, setTaskDetails, setTasks,
} from '../../reducers/data'
import { PageMessage } from '../Page'
import Datum, { DatumComment, DatumPriority, DatumValue, DatumStatus } from '../Datum'
import { Loader, Textarea, Button } from '../../shared'
import { PRIORITIES } from '../../shared/consts'
import { fetchTasks, finishTask, sendComment } from '../../services/task'
import { useToasts } from 'react-toast-notifications'

const TaskDetails = function () {
  const match = useRouteMatch()
  const id = +match.params.task
  const dispatch = useDispatch()
  const details = useSelector(({ data }) => data.details, shallowEqual)
  const userId = useSelector(({ user }) => user.id)
  const data = details || {}
  const [ comment, setComment] = useState('')
  const [ isDisabled, setDisabled ] = useState(false)
  const { addToast } = useToasts()
  
  useEffect(function () {
    dispatch(loadTaskDetails(id))
  }, [dispatch, id ])
  
  const handleSendComment = async function () {
    try {
      setDisabled(true)
      const data = await sendComment(userId, id, comment)
      dispatch(addComment(data))
      setComment('')
      setDisabled(false)
    } catch {
      setDisabled(false)
      addToast('Не удалось отправить комментарий', {
        appearance: 'error',
        autoDismissTimeout: 3000,
        autoDismiss: true
      })
    }
  }
  
  const handleFinishTask = async function () {
    try {
      setDisabled(true)
      const data = await finishTask(userId, id, comment)
      const tasks = await fetchTasks(userId, match.params.page)
      dispatch(setTaskDetails(data))
      dispatch(setTasks(match.params.page, tasks))
      setComment('')
      setDisabled(false)
    } catch {
      setDisabled(false)
      addToast('Не удалось отправить комментарий', {
        appearance: 'error',
        autoDismissTimeout: 3000,
        autoDismiss: true
      })
    }
  }
  
  if (!id) {
    return <PageMessage>
      Выберите задачу
    </PageMessage>
  }
  
  if (details === null) {
    return <PageMessage>
      Задача не найдена
    </PageMessage>
  }
  
  return <div>
    <div style={{
      fontSize: '16px',
      margin: '0 0 10px 0',
      color: '#000'
    }}>
      {data.name === undefined ? <Loader /> : data.name}
    </div>
    <Datum label='Создана'>
      <DatumValue value={data.created} type='date' />
      <DatumValue value={data.creatorId} type='person' />
    </Datum>

    <Datum label='Объект'>
      <DatumValue value={data.taskObject} />
    </Datum>
    
    <Datum label='Исполнитель'>
      <DatumValue value={data.executorId} type='person' />
    </Datum>

    <Datum label='Исполнить до'>
      <DatumValue value={data.plannedAt} type='date' />
      <DatumValue value={data.priority !== undefined 
        ? <>
          <span>Приоритет: &nbsp;</span>
          <span>
            <DatumPriority priority={data.priority} />
            <span>&nbsp;&nbsp;{PRIORITIES[data.priority]}</span>
          </span>
        </>
        : undefined
      } />
    </Datum>

    <Datum label='Статус'>
      <DatumValue value={data.isFinished !== undefined
        ? data.isFinished
          ? <DatumStatus className={'-success'}>Завершено</DatumStatus>
          : <DatumStatus>Активно</DatumStatus>
        : undefined
      } />
      {data.isFinished && <DatumValue value={data.finishedAt} type='date' /> }
      
    </Datum>

    <Datum label='Описание' large>
      <DatumValue value={data.description} />
    </Datum>

    <Datum label='Комментарии' large>
      {data.comments && data.comments.map(comment => 
        <DatumComment {...comment} key={comment.id}/>
      )}
      {data.finishComment && <DatumComment text={data.finishComment} />}
      <dd>
        <Textarea 
          placeholder='Добавить комментарий' 
          style={{ marginBottom: '5px'}}
          value={comment}
          onChange={e => setComment(e.currentTarget.value)}
          disabled={isDisabled}
        />
        <Button 
          light 
          onClick={handleSendComment}
          disabled={!comment || isDisabled}
        >
          Отправить
        </Button>
        <Button
          onClick={handleFinishTask}
          disabled={data.isFinished || isDisabled}
        >
          Завершить задачу
        </Button>
      </dd>
    </Datum>
  </div>
}

export default TaskDetails