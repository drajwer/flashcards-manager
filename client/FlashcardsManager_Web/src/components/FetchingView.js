import React from 'react';
import * as Ons from 'react-onsenui';

export default class FetchingView extends React.Component {
    render() {
        return (
            <div style={{textAlign: 'center', margin: '20px'}}>
                <Ons.ProgressCircular indeterminate/>
            </div>

        )
    }
}