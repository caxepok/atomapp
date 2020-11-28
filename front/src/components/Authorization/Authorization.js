import React, { useEffect} from 'react'
import { shallowEqual, useDispatch, useSelector } from 'react-redux'
import { getWorkplaces } from '../../reducers/data'
import LayoutSection from '../../components/Layout/LayoutSection'
import WorkplacesItem from '../Workplaces/WorkplacesItem'
import Workplaces from '../Workplaces'
import { setUser } from '../../reducers/user'

const Authorization = function (props) {
  const dispatch = useDispatch()
  const workplaces = useSelector(({ data}) => data.workplaces, shallowEqual)
  const userId = useSelector(({ user }) => user && user.id)

  useEffect(function () {
    dispatch(getWorkplaces())
    const savedUser = localStorage.getItem('user')
    if (savedUser) {
      dispatch(setUser(JSON.parse(savedUser)))
    }
  }, [])
  
  return userId 
    ? props.children 
    : <>
      <LayoutSection
        menu
        header={<span>Подразделения</span>}
      >
        <ul className='workplaces-tree'>
          {workplaces && workplaces.map((item, key) => (
            <WorkplacesItem key={item.id} {...item} />
          ))}
        </ul>
      </LayoutSection>
      <Workplaces />
    </>
}

export default Authorization