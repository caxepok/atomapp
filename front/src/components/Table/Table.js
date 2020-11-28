import React from 'react'
import classes from 'classnames'
import './table.scss'
import { useDispatch } from 'react-redux'
import { setRedirect } from '../../reducers/layout'

const Table = function (props) {
  const { data, sizes, labels } = props
  const dispatch = useDispatch()
  
  if (!data) {
    return null
  }
  
  const handleClick = function (to) {
    dispatch(setRedirect(to))
  }
  
  return <div className='table'>
    <table>
      {labels && <thead>
      <tr>
        {labels.map((cell, key) => <th key={key}>
          { cell }
        </th>)}
      </tr>
      </thead>}
      <tbody>
      {data.map((row, index) => <tr
        key={index}
        onClick={row.to ? () => handleClick(row.to) : undefined}
        className={classes(row.to && '-clickable', row.selected && '-selected')}
      >
        {Object.values(row.datum).map((cell, index) => (
          <td key={index} width={sizes && sizes[index]}>
            {cell}
          </td>
        ))}
      </tr>)}
      </tbody>
    </table>
  </div>
}

export default Table