import React from 'react';

import * as Ons from 'react-onsenui';

import NavBar from './NavBar';
import LocationList from '../containers/LocationList';
import AddLocation from '../containers/AddLocation';
import {colors} from '../util/colors';
export default class SelectCategoryView extends React.Component {
  renderRow = (category, index) => {
    const {onCategorySelect} = this.props;
    const color = colors[index % colors.length];
    return (
      <Ons.ListItem key={index} tappable onClick={() => onCategorySelect(category.id)}>
        <div className='left'>
        <span style={{background: color, width: '40px', height: '40px', borderRadius: '50%'}} className='list-item_thumbnail' />
      </div>
        <div className='center'>
          {category.name}
        </div>
      </Ons.ListItem>
    )
  };
  render() {
    const {categories} = this.props;
    return (
      <div>
        <Ons.List
          dataSource={categories}
          renderRow={this.renderRow}
          renderHeader={() => <Ons.ListHeader>Wszystkie kategorie</Ons.ListHeader>}
        />
      </div>
    )
  }
}